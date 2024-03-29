﻿using Aggregates;
using Aggregates.Application;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Ordering.Order
{
    public class SalesWeekOverWeek :
        IHandleQueries<Queries.SalesWeekOverWeek>,
        IHandleMessages<Events.Drafted>,
        IHandleMessages<Events.Canceled>,
        IHandleMessages<Entities.Item.Events.Added>,
        IHandleMessages<Entities.Item.Events.PriceOverridden>,
        IHandleMessages<Entities.Item.Events.Removed>
    {
        // make a day of week id with order day's year, month, and first day of the week
        public static Func<DateTime, string> IdGenerator = (orderDay) => $"{orderDay.StartOfWeek().ToString("yyyy.MM.dd")}.{orderDay.DayOfWeek}";

        public async Task Handle(Queries.SalesWeekOverWeek query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            if (query.From.HasValue)
                builder.Add("Relevancy", query.From.Value.ToUnix().ToString(), Operation.GreaterThanOrEqual);
            if (query.To.HasValue)
                builder.Add("Relevancy", query.To.Value.ToUnix().ToString(), Operation.LessThanOrEqual);

            var results = await ctx.Uow().Query<Models.SalesWeekOverWeek>(builder.Build())
                .ConfigureAwait(false);

            var records = results.Records.GroupBy(x => x.DayOfWeek).Select(x => new Models.SalesWeekOverWeek
            {
                Id = x.First().Id,
                Relevancy = x.First().Relevancy,
                DayOfWeek = x.Key,
                Value = x.Sum(g => g.Value)
            }).ToArray();

            await ctx.Result(records, records.Count(), results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Events.Drafted e, IMessageHandlerContext ctx)
        {
            var day = e.Stamp.FromUnix().Date;
            
            // get all items in basket
            var itemIds = await ctx.Service<Basket.Basket.Entities.Item.Services.ItemsInBasket, string[]>(x => { x.BasketId = e.BasketId; })
                .ConfigureAwait(false);

            var items = await itemIds.SelectAsync(id =>
            {
                return ctx.Uow().Get<Basket.Basket.Entities.Item.Models.BasketItemIndex>(id);
            }).ConfigureAwait(false);

            var existing = await ctx.Uow().TryGet<Models.SalesWeekOverWeek>(IdGenerator(day)).ConfigureAwait(false);
            if (existing == null)
            {
                existing = new Models.SalesWeekOverWeek
                {
                    Id = IdGenerator(day),
                    Relevancy = day.StartOfWeek().ToUnix(),
                    DayOfWeek = day.DayOfWeek.ToString(),
                    Value = items.Sum(x => x.SubTotal)
                };
                await ctx.Uow().Add(existing.Id, existing).ConfigureAwait(false);
            }
            else
            {
                // todo: add additional fees when additional fees exist
                existing.Value += items.Sum(x => x.SubTotal);
                await ctx.Uow().Update(existing.Id, existing).ConfigureAwait(false);
            }
        }
        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.Uow().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;

            var existing = await ctx.Uow().Get<Models.SalesWeekOverWeek>(IdGenerator(day)).ConfigureAwait(false);
            existing.Value -= order.Total;
            await ctx.Uow().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Added e, IMessageHandlerContext ctx)
        {
            var order = await ctx.Uow().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var product = await ctx.Uow().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;

            var existing = await ctx.Uow().Get<Models.SalesWeekOverWeek>(IdGenerator(day)).ConfigureAwait(false);
            existing.Value += product.Price * e.Quantity;
            await ctx.Uow().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.PriceOverridden e, IMessageHandlerContext ctx)
        {
            var order = await ctx.Uow().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var orderItem = await ctx.Uow().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;

            var existing = await ctx.Uow().Get<Models.SalesWeekOverWeek>(IdGenerator(day)).ConfigureAwait(false);
            // remove existing value
            existing.Value -= orderItem.Total;

            orderItem.Price = e.Price;

            // add updated total
            existing.Value += orderItem.Total;

            await ctx.Uow().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Removed e, IMessageHandlerContext ctx)
        {
            var order = await ctx.Uow().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var orderItem = await ctx.Uow().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;

            var existing = await ctx.Uow().Get<Models.SalesWeekOverWeek>(IdGenerator(day)).ConfigureAwait(false);
            // remove existing value
            existing.Value -= orderItem.Total;

            await ctx.Uow().Update(existing.Id, existing).ConfigureAwait(false);
        }
    }
}
