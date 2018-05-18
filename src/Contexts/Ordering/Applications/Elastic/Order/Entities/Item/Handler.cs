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

namespace eShop.Ordering.Order.Entities.Item
{
    public class Handler :
        IHandleQueries<Queries.Items>,
        IHandleMessages<Order.Events.Drafted>,
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.PriceOverridden>,
        IHandleMessages<Events.Removed>
    {
        public static Func<Guid, Guid, string> ItemIdGenerator = (orderId, productId) => $"{orderId}.{productId}";

        public async Task Handle(Queries.Items query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            builder.Add("OrderId", query.OrderId.ToString(), Operation.EQUAL);

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.OrderingOrderItem>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Order.Events.Drafted e, IMessageHandlerContext ctx)
        {
            // get all items in basket
            var itemIds = await ctx.Service<Basket.Basket.Entities.Item.Services.ItemsInBasket, Guid[]>(x => { x.BasketId = e.BasketId; })
                .ConfigureAwait(false);

            // add the items to order
            foreach (var id in itemIds)
            {
                var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Basket.Basket.Entities.Item.Models.BasketItem>(id).ConfigureAwait(false);

                var model = new Models.OrderingOrderItem
                {
                    Id = ItemIdGenerator(e.OrderId, item.ProductId),
                    OrderId = e.OrderId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductDescription = item.ProductDescription,
                    ProductPictureContents = item.ProductPictureContents,
                    ProductPictureContentType = item.ProductPictureContentType,
                    ProductPrice = item.ProductPrice,
                    Quantity = item.Quantity,
                };

                await ctx.App<Infrastructure.IUnitOfWork>().Add(model.Id, model).ConfigureAwait(false);
            }
        }

        public async Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Catalog.Product.Models.CatalogProduct>(e.ProductId).ConfigureAwait(false);

            var model = new Models.OrderingOrderItem
            {
                Id = ItemIdGenerator(e.OrderId, e.ProductId),
                OrderId = e.OrderId,
                ProductId = e.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPictureContents = product.PictureContents,
                ProductPictureContentType = product.PictureContentType,
                ProductPrice = product.Price,
                Quantity = e.Quantity
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(model.Id, model).ConfigureAwait(false);
        }

        public async Task Handle(Events.PriceOverridden e, IMessageHandlerContext ctx)
        {
            var orderitem = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrderItem>(ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);
            orderitem.Price = e.Price;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(orderitem.Id, orderitem).ConfigureAwait(false);
        }

        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.OrderingOrderItem>(ItemIdGenerator(e.OrderId, e.ProductId));
        }
    }
}
