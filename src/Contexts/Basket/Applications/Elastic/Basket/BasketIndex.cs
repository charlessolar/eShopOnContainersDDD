using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Basket.Basket
{
    public class BasketIndex :
        IHandleQueries<Queries.Baskets>,
        IHandleMessages<Events.Initiated>,
        IHandleMessages<Events.BasketClaimed>,
        IHandleMessages<Events.Destroyed>,
        IHandleMessages<Entities.Item.Events.ItemAdded>,
        IHandleMessages<Entities.Item.Events.ItemRemoved>,
        IHandleMessages<Entities.Item.Events.QuantityUpdated>,
        IHandleMessages<Catalog.Product.Events.PriceUpdated>
    {
        public async Task Handle(Queries.Baskets query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            var results = await ctx.UoW().Query<Models.Basket>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Events.Initiated e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().TryGet<Identity.User.Models.User>(e.UserName)
                .ConfigureAwait(false);

            var basket = new Models.BasketIndex
            {
                Id = e.BasketId,
                CustomerId = user?.Id,
                Customer = user?.GivenName,
                Created = e.Stamp,
                Updated = e.Stamp
            };
            await ctx.UoW().Add(e.BasketId, basket).ConfigureAwait(false);
        }
        public async Task Handle(Events.BasketClaimed e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().Get<Identity.User.Models.User>(e.UserName)
                .ConfigureAwait(false);
            var basket = await ctx.UoW().Get<Models.BasketIndex>(e.BasketId)
                .ConfigureAwait(false);

            basket.Customer = user.GivenName;
            basket.CustomerId = user.Id;

            basket.Updated = e.Stamp;

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.BasketIndex>(e.BasketId);
        }

        public async Task Handle(Entities.Item.Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.BasketIndex>(e.BasketId)
                .ConfigureAwait(false);
            var product = await ctx.UoW()
                .Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);
            basket.TotalItems++;
            basket.TotalQuantity++;
            basket.SubTotal += product.Price;

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Entities.Item.Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.BasketIndex>(e.BasketId)
                .ConfigureAwait(false);
            var item = await ctx.UoW()
                .Get<Entities.Item.Models.BasketItemIndex>(Entities.Item.BasketItemIndex.ItemIdGenerator(e.BasketId, e.ProductId)).ConfigureAwait(false);

            basket.TotalItems--;
            basket.TotalQuantity -= item.Quantity;
            basket.SubTotal -= item.SubTotal;

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Entities.Item.Events.QuantityUpdated e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.BasketIndex>(e.BasketId)
                .ConfigureAwait(false);
            var item = await ctx.UoW()
                .Get<Entities.Item.Models.BasketItemIndex>(Entities.Item.BasketItemIndex.ItemIdGenerator(e.BasketId, e.ProductId)).ConfigureAwait(false);

            // Todo: verify item is the item state before IT processes QuantityUpdated
            basket.TotalQuantity -= item.Quantity;
            basket.SubTotal -= item.SubTotal;

            item.Quantity = e.Quantity;

            basket.TotalQuantity += item.Quantity;
            basket.SubTotal += item.SubTotal;

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Catalog.Product.Events.PriceUpdated e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Services.BasketsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the subtotal and quantity for all baskets
            foreach (var id in basketIds)
            {
                var basket = await ctx.UoW().Get<Models.BasketIndex>(id)
                    .ConfigureAwait(false);
                var item = await ctx.UoW()
                    .Get<Entities.Item.Models.BasketItemIndex>(Entities.Item.BasketItemIndex.ItemIdGenerator(id, e.ProductId)).ConfigureAwait(false);

                basket.SubTotal -= item.SubTotal;

                item.ProductPrice = e.Price;

                basket.SubTotal += item.SubTotal;

                await ctx.UoW().Update(id, basket).ConfigureAwait(false);
            }

        }
    }

}
