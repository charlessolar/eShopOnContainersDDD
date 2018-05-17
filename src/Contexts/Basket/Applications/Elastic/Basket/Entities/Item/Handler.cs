using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Basket.Basket.Entities.Item
{
    public class Handler :
        IHandleQueries<Queries.Items>,
        IHandleMessages<Events.ItemAdded>,
        IHandleMessages<Events.ItemRemoved>,
        IHandleMessages<Events.QuantityUpdated>,
        IHandleMessages<Catalog.Product.Events.DescriptionUpdated>,
        IHandleMessages<Catalog.Product.Events.PictureSet>,
        IHandleMessages<Catalog.Product.Events.PriceUpdated>
    {
        public static Func<Guid, Guid, string> ItemIdGenerator = (basketId, productId) => $"{basketId}.{productId}";

        public async Task Handle(Queries.Items query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            builder.Add("BasketId", query.BasketId.ToString(), Operation.EQUAL);

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.BasketItemIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId)
                .ConfigureAwait(false);
            var model = new Models.BasketItemIndex
            {
                Id = ItemIdGenerator(e.BasketId, e.ProductId),
                BasketId = e.BasketId,
                ProductId = e.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPictureContents = product.PictureContents,
                ProductPictureContentType = product.PictureContentType,
                ProductPrice = product.Price,
                Quantity = 1,
            };
            await ctx.App<Infrastructure.IUnitOfWork>().Add(model.Id, model).ConfigureAwait(false);
        }
        public Task Handle(Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.BasketItemIndex>(ItemIdGenerator(e.BasketId, e.ProductId));
        }
        public async Task Handle(Events.QuantityUpdated e, IMessageHandlerContext ctx)
        {
            var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.BasketItemIndex>(ItemIdGenerator(e.BasketId, e.ProductId)).ConfigureAwait(false);

            item.Quantity = e.Quantity;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(ItemIdGenerator(e.BasketId, e.ProductId), item).ConfigureAwait(false);
        }
        public async Task Handle(Catalog.Product.Events.DescriptionUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId)
                .ConfigureAwait(false);

            var basketIds = await ctx.Service<Services.ItemsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the description for all baskets
            foreach (var id in basketIds)
            {
                var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.BasketItemIndex>(ItemIdGenerator(id, e.ProductId)).ConfigureAwait(false);
                item.ProductDescription = e.Description;

                await ctx.App<Infrastructure.IUnitOfWork>().Update(ItemIdGenerator(id, e.ProductId), item).ConfigureAwait(false);
            }
        }
        public async Task Handle(Catalog.Product.Events.PictureSet e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Services.ItemsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the description for all baskets
            foreach (var id in basketIds)
            {
                var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.BasketItemIndex>(ItemIdGenerator(id, e.ProductId)).ConfigureAwait(false);
                item.ProductPictureContents = e.Content;
                item.ProductPictureContentType = e.ContentType;

                await ctx.App<Infrastructure.IUnitOfWork>().Update(ItemIdGenerator(id, e.ProductId), item).ConfigureAwait(false);
            }
        }
        public async Task Handle(Catalog.Product.Events.PriceUpdated e, IMessageHandlerContext ctx)
        {

            var basketIds = await ctx.Service<Services.ItemsUsingProduct, Guid[]>(x => { x.ProductId = e.ProductId; })
                .ConfigureAwait(false);

            // Update the description for all baskets
            foreach (var id in basketIds)
            {
                var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.BasketItemIndex>(ItemIdGenerator(id, e.ProductId)).ConfigureAwait(false);
                item.ProductPrice = e.Price;

                await ctx.App<Infrastructure.IUnitOfWork>().Update(ItemIdGenerator(id, e.ProductId), item).ConfigureAwait(false);
            }
        }

    }
}
