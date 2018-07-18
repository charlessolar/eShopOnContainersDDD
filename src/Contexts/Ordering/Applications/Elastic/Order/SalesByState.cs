using Aggregates;
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
    public class SalesByState :
        IHandleQueries<Queries.SalesByState>,
        IHandleMessages<Events.Drafted>,
        IHandleMessages<Events.Canceled>,
        IHandleMessages<Entities.Item.Events.Added>,
        IHandleMessages<Entities.Item.Events.PriceOverridden>,
        IHandleMessages<Entities.Item.Events.Removed>
    {
        // make a state id with order day's month and year
        public static Func<DateTime, string, string> IdGenerator = (orderDay, state) => $"{orderDay.Month}.{orderDay.Year}.{state}";

        public async Task Handle(Queries.SalesByState query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            if (query.From.HasValue)
                builder.Add("Relevancy", query.From.Value.ToUnix().ToString(), Operation.GreaterThanOrEqual);
            if (query.To.HasValue)
                builder.Add("Relevancy", query.To.Value.ToUnix().ToString(), Operation.LessThanOrEqual);

            var results = await ctx.UoW().Query<Models.SalesByState>(builder.Build())
                .ConfigureAwait(false);

            var records = results.Records.GroupBy(x => x.State).Select(x => new Models.SalesByState
            {
                Id = x.First().Id,
                Relevancy = x.First().Relevancy,
                State = x.Key,
                Value = x.Sum(g => g.Value)
            }).ToArray();

            await ctx.Result(records, records.Count(), results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Events.Drafted e, IMessageHandlerContext ctx)
        {
            var day = e.Stamp.FromUnix().Date;
            var month = new DateTime(day.Year, day.Month, 1);

            var address = await ctx.UoW().Get<Buyer.Entities.Address.Models.Address>(e.ShippingAddressId).ConfigureAwait(false);

            // get all items in basket
            var itemIds = await ctx.Service<Basket.Basket.Entities.Item.Services.ItemsInBasket, string[]>(x => { x.BasketId = e.BasketId; })
                .ConfigureAwait(false);

            var items = await itemIds.SelectAsync(id =>
            {
                return ctx.UoW().Get<Basket.Basket.Entities.Item.Models.BasketItemIndex>(id);
            }).ConfigureAwait(false);

            var existing = await ctx.UoW().TryGet<Models.SalesByState>(IdGenerator(month, address.State)).ConfigureAwait(false);
            if (existing == null)
            {
                existing = new Models.SalesByState
                {
                    Id = IdGenerator(month, address.State),
                    Relevancy = month.ToUnix(),
                    State = address.State,
                    Value = items.Sum(x => x.SubTotal)
                };
                await ctx.UoW().Add(existing.Id, existing).ConfigureAwait(false);
            }
            else
            {
                // todo: add additional fees when additional fees exist
                existing.Value += items.Sum(x => x.SubTotal);
                await ctx.UoW().Update(existing.Id, existing).ConfigureAwait(false);
            }
        }
        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;
            var month = new DateTime(day.Year, day.Month, 1);

            var existing = await ctx.UoW().Get<Models.SalesByState>(IdGenerator(month, order.ShippingState)).ConfigureAwait(false);
            existing.Value -= order.Total;
            await ctx.UoW().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Added e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var product = await ctx.UoW().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;
            var month = new DateTime(day.Year, day.Month, 1);

            var existing = await ctx.UoW().Get<Models.SalesByState>(IdGenerator(month, order.ShippingState)).ConfigureAwait(false);
            existing.Value += product.Price * e.Quantity;
            await ctx.UoW().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.PriceOverridden e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var orderItem = await ctx.UoW().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;
            var month = new DateTime(day.Year, day.Month, 1);

            var existing = await ctx.UoW().Get<Models.SalesByState>(IdGenerator(month, order.ShippingState)).ConfigureAwait(false);
            // remove existing value
            existing.Value -= orderItem.Total;

            orderItem.Price = e.Price;

            // add updated total
            existing.Value += orderItem.Total;

            await ctx.UoW().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Removed e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var orderItem = await ctx.UoW().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            var day = order.Created.FromUnix().Date;
            var month = new DateTime(day.Year, day.Month, 1);

            var existing = await ctx.UoW().Get<Models.SalesByState>(IdGenerator(month, order.ShippingState)).ConfigureAwait(false);
            // remove existing value
            existing.Value -= orderItem.Total;

            await ctx.UoW().Update(existing.Id, existing).ConfigureAwait(false);
        }
    }
}
