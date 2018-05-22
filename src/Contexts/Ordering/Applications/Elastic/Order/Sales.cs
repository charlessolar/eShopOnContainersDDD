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
    public class Sales :
        IHandleQueries<Queries.Sales>,
        IHandleMessages<Events.Drafted>,
        IHandleMessages<Events.Canceled>,
        IHandleMessages<Entities.Item.Events.Added>,
        IHandleMessages<Entities.Item.Events.PriceOverridden>,
        IHandleMessages<Entities.Item.Events.Removed>
    {

        // flattens order timestamp into just the day
        public static Func<DateTime, string> IdGenerator = (orderDay) => $"{orderDay.ToUnix()}";

        public async Task Handle(Queries.Sales query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            if (query.From.HasValue)
                builder.Add("Relevancy", query.From.Value.ToUnix().ToString(), Operation.GREATER_THAN_OR_EQUAL);
            if (query.To.HasValue)
                builder.Add("Relevancy", query.To.Value.ToUnix().ToString(), Operation.LESS_THAN_OR_EQUAL);

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.SalesChart>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Events.Drafted e, IMessageHandlerContext ctx)
        {
            var day = e.Stamp.FromUnix().Date;

            // get all items in basket
            var itemIds = await ctx.Service<Basket.Basket.Entities.Item.Services.ItemsInBasket, string[]>(x => { x.BasketId = e.BasketId; })
                .ConfigureAwait(false);

            var items = await itemIds.SelectAsync(id =>
            {
                return ctx.App<Infrastructure.IUnitOfWork>().Get<Basket.Basket.Entities.Item.Models.BasketItemIndex>(id);
            }).ConfigureAwait(false);

            var existing = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.SalesChart>(IdGenerator(day)).ConfigureAwait(false);
            if(existing == null)
            {
                existing = new Models.SalesChart
                {
                    Id = IdGenerator(day),
                    Relevancy = day.ToUnix(),
                    Label = day.ToString("s"),
                    Value = items.Sum(x => x.SubTotal)
                };
                await ctx.App<Infrastructure.IUnitOfWork>().Add(existing.Id, existing).ConfigureAwait(false);
            }
            else
            {
                // todo: add additional fees when additional fees exist
                existing.Value += items.Sum(x => x.SubTotal);
                await ctx.App<Infrastructure.IUnitOfWork>().Update(existing.Id, existing).ConfigureAwait(false);
            }
        }
        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            var existing = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.SalesChart>(IdGenerator(order.Created.FromUnix().Date)).ConfigureAwait(false);
            existing.Value -= order.Total;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Added e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            var existing = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.SalesChart>(IdGenerator(order.Created.FromUnix().Date)).ConfigureAwait(false);
            existing.Value += product.Price * e.Quantity;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.PriceOverridden e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var orderItem = await ctx.App<Infrastructure.IUnitOfWork>().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            var existing = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.SalesChart>(IdGenerator(order.Created.FromUnix().Date)).ConfigureAwait(false);
            // remove existing value
            existing.Value -= orderItem.Total;

            orderItem.Price = e.Price;

            // add updated total
            existing.Value += orderItem.Total;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(existing.Id, existing).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Removed e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var orderItem = await ctx.App<Infrastructure.IUnitOfWork>().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            var existing = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.SalesChart>(IdGenerator(order.Created.FromUnix().Date)).ConfigureAwait(false);
            // remove existing value
            existing.Value -= orderItem.Total;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(existing.Id, existing).ConfigureAwait(false);
        }
    }
}
