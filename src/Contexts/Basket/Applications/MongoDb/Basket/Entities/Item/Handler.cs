using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Basket.Basket.Entities.Item
{
    public class Handler : 
        IHandleMessages<Events.ItemAdded>,
        IHandleMessages<Events.ItemRemoved>,
        IHandleMessages<Events.QuantityUpdated>
    {
        public async Task Handle(Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Catalog.Product.Models.Product>(e.ProductId)
                .ConfigureAwait(false);
            var model = new Models.Items
            {
                BasketId = e.BasketId,
                ItemId = e.ItemId,
                ProductId = e.ProductId,
                ProductName = product.Name,
                ProductPrice = product.Price,
                Quantity = e.Quantity,
                Total = product.Price * e.Quantity
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.ItemId, model).ConfigureAwait(false);
        }

        public Task Handle(Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.Items>(e.ItemId);
        }

        public async Task Handle(Events.QuantityUpdated e, IMessageHandlerContext ctx)
        {
            var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Items>(e.ItemId).ConfigureAwait(false);

            item.Quantity = e.Quantity;
            item.Total = item.Quantity * item.ProductPrice;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.ItemId, item).ConfigureAwait(false);
        }
    }
}
