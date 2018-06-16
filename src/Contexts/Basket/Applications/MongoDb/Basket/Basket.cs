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
    public class Basket : 
        IHandleQueries<Queries.Basket>,
        IHandleMessages<Events.Initiated>,
        IHandleMessages<Events.BasketClaimed>,
        IHandleMessages<Events.Destroyed>,
        IHandleMessages<Entities.Item.Events.ItemAdded>,
        IHandleMessages<Entities.Item.Events.ItemRemoved>,
        IHandleMessages<Entities.Item.Events.QuantityUpdated>,
        IHandleMessages<Catalog.Product.Events.PriceUpdated>,
        IHandleMessages<Catalog.Product.Events.DescriptionUpdated>,
        IHandleMessages<Catalog.Product.Events.PictureSet>
    {
        public static Func<Guid, Guid, string> ItemIdGenerator = (basketId, productId) => $"{basketId}.{productId}";

        public async Task Handle(Queries.Basket query, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.Basket>(query.BasketId)
                .ConfigureAwait(false);

            await ctx.Result(basket).ConfigureAwait(false);
        }
        public async Task Handle(Events.Initiated e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().TryGet<Identity.User.Models.User>(e.UserName)
                .ConfigureAwait(false);

            var basket = new Models.Basket
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
            var basket = await ctx.UoW().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);

            basket.Customer = user.GivenName;
            basket.CustomerId = user.Id;

            basket.Updated = e.Stamp;

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.Basket>(e.BasketId);
        }
        public async Task Handle(Entities.Item.Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);
            var product = await ctx.UoW()
                .Get<Catalog.Product.Models.CatalogProduct>(e.ProductId).ConfigureAwait(false);

            basket.Items = basket.Items.TryAdd(new Entities.Item.Models.BasketItem
            {
                Id = ItemIdGenerator(e.BasketId, e.ProductId),
                ProductId = e.ProductId,
                BasketId = e.BasketId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPictureContents = product.PictureContents,
                ProductPictureContentType = product.PictureContentType,
                ProductPrice = product.Price,
                Quantity = 1
            }, x => x.Id);

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Entities.Item.Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);
            basket.Items = basket.Items.TryRemove(ItemIdGenerator(e.BasketId, e.ProductId), x => x.Id);

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Entities.Item.Events.QuantityUpdated e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.Basket>(e.BasketId)
                .ConfigureAwait(false);

            var item = basket.Items.Single(x => x.ProductId == e.ProductId);

            item.Quantity = e.Quantity;

            await ctx.UoW().Update(e.BasketId, basket).ConfigureAwait(false);
        }

        public async Task Handle(Catalog.Product.Events.PriceUpdated e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Services.BasketsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the subtotal and quantity for all baskets
            foreach (var id in basketIds)
            {
                var basket = await ctx.UoW().Get<Models.Basket>(id)
                    .ConfigureAwait(false);
                var item = basket.Items.Single(x => x.ProductId == e.ProductId);
                
                item.ProductPrice = e.Price;

                await ctx.UoW().Update(id, basket).ConfigureAwait(false);
            }
        }
        public async Task Handle(Catalog.Product.Events.PictureSet e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Services.BasketsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the subtotal and quantity for all baskets
            foreach (var id in basketIds)
            {
                var basket = await ctx.UoW().Get<Models.Basket>(id)
                    .ConfigureAwait(false);
                var item = basket.Items.Single(x => x.ProductId == e.ProductId);

                item.ProductPictureContents = e.Content;
                item.ProductPictureContentType = e.ContentType;

                await ctx.UoW().Update(id, basket).ConfigureAwait(false);
            }
        }
        public async Task Handle(Catalog.Product.Events.DescriptionUpdated e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Services.BasketsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the subtotal and quantity for all baskets
            foreach (var id in basketIds)
            {
                var basket = await ctx.UoW().Get<Models.Basket>(id)
                    .ConfigureAwait(false);
                var item = basket.Items.Single(x => x.ProductId == e.ProductId);

                item.ProductDescription = e.Description;

                await ctx.UoW().Update(id, basket).ConfigureAwait(false);
            }
        }
    }
}
