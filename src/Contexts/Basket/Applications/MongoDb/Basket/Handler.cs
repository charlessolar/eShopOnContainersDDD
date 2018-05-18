using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Basket.Basket
{
    public class Handler : 
        IHandleQueries<Queries.Basket>,
        IHandleMessages<Events.Initiated>,
        IHandleMessages<Events.BasketClaimed>,
        IHandleMessages<Events.Destroyed>,
        IHandleMessages<Entities.Item.Events.ItemAdded>,
        IHandleMessages<Entities.Item.Events.ItemRemoved>,
        IHandleMessages<Entities.Item.Events.QuantityUpdated>,
        IHandleMessages<Catalog.Product.Events.PriceUpdated>
    {
        public async Task Handle(Queries.Basket query, IMessageHandlerContext ctx)
        {
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Basket>(query.BasketId)
                .ConfigureAwait(false);

            await ctx.Result(basket).ConfigureAwait(false);
        }
        public async Task Handle(Events.Initiated e, IMessageHandlerContext ctx)
        {
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Identity.User.Models.User>(e.UserName)
                .ConfigureAwait(false);

            var basket = new Models.Basket
            {
                Id = e.BasketId,
                CustomerId = user?.Id,
                Customer = user?.GivenName,
                Created = e.Stamp,
                Updated = e.Stamp
            };
            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.BasketId, basket).ConfigureAwait(false);
        }
        public async Task Handle(Events.BasketClaimed e, IMessageHandlerContext ctx)
        {
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Identity.User.Models.User>(e.UserName)
                .ConfigureAwait(false);
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);

            basket.Customer = user.GivenName;
            basket.CustomerId = user.Id;

            basket.Updated = e.Stamp;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.BasketId, basket).ConfigureAwait(false);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.Basket>(e.BasketId);
        }
        public async Task Handle(Entities.Item.Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);
            var product = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Catalog.Product.Models.CatalogProduct>(e.ProductId).ConfigureAwait(false);
            basket.TotalItems++;
            basket.TotalQuantity++;
            basket.SubTotal += product.Price;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Entities.Item.Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);
            var item = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Entities.Item.Models.BasketItem>(Entities.Item.Handler.ItemIdGenerator(e.BasketId, e.ProductId)).ConfigureAwait(false);

            basket.TotalItems--;
            basket.TotalQuantity -= item.Quantity;
            basket.SubTotal -= item.SubTotal;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Entities.Item.Events.QuantityUpdated e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);
            var item = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Entities.Item.Models.BasketItem>(Entities.Item.Handler.ItemIdGenerator(e.BasketId, e.ProductId)).ConfigureAwait(false);

            // Todo: verify item is the item state before IT processes QuantityUpdated
            basket.TotalQuantity -= item.Quantity;
            basket.SubTotal -= item.SubTotal;

            item.Quantity = e.Quantity;

            basket.TotalQuantity += item.Quantity;
            basket.SubTotal += item.SubTotal;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Catalog.Product.Events.PriceUpdated e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Entities.Item.Services.ItemsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the subtotal and quantity for all baskets
            foreach (var id in basketIds)
            {
                var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Entities.Item.Models.BasketItem>(Entities.Item.Handler.ItemIdGenerator(id, e.ProductId)).ConfigureAwait(false);
                var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Basket>(id)
                    .ConfigureAwait(false);

                basket.SubTotal -= item.SubTotal;

                item.ProductPrice = e.Price;

                basket.SubTotal += item.SubTotal;

                await ctx.App<Infrastructure.IUnitOfWork>().Update(id, basket).ConfigureAwait(false);
            }
        }
    }
}
