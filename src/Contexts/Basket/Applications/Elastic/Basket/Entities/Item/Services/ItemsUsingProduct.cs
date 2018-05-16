using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Extensions;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    public class ItemsUsingProduct : IService<Guid[]>
    {
        public Guid ProductId { get; set; }
    }

    public class ItemsUsingProductHandler :
        IHandleMessages<Item.Events.ItemAdded>,
        IHandleMessages<Item.Events.ItemRemoved>,
        IHandleMessages<Basket.Events.Destroyed>,
        IHandleMessages<Catalog.Product.Events.Removed>,
        IProvideService<ItemsUsingProduct, Guid[]>
    {
        // Todo: note that as more and more baskets happen these lists can grow pretty big.  Obviously this is a small scale solution
        // it is possible to do queries in mongo so it wouldn't be hard to stop doing this
        public async Task Handle(Item.Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var productitems = await ctx.App<Infrastructure.IUnitOfWork>().Get<ProductItems>(e.ProductId)
                .ConfigureAwait(false);
            if (productitems == null)
            {
                productitems = new ProductItems
                {
                    ProductId = e.ProductId,
                    Baskets = new[] {e.BasketId}
                };
                await ctx.App<Infrastructure.IUnitOfWork>().Add(e.ProductId, productitems).ConfigureAwait(false);
            }
            else
            {
                productitems.Baskets = productitems.Baskets.TryAdd(e.BasketId);
                await ctx.App<Infrastructure.IUnitOfWork>().Update(e.ProductId, productitems).ConfigureAwait(false);
            }
        }

        public async Task Handle(Item.Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            var productitems = await ctx.App<Infrastructure.IUnitOfWork>().Get<ProductItems>(e.ProductId)
                .ConfigureAwait(false);

            productitems.Baskets = productitems.Baskets.TryRemove(e.BasketId);
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.ProductId, productitems).ConfigureAwait(false);
        }

        public Task Handle(Catalog.Product.Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<ProductItems>(e.ProductId);
        }

        // when a basket is destroyed, remove the basket from all ProductItems
        public async Task Handle(Basket.Events.Destroyed e, IMessageHandlerContext ctx)
        {
            // get all items in basket
            var itemIds = await ctx.Service<Services.ItemsInBasket, Guid[]>(x => { x.BasketId= e.BasketId; })
                .ConfigureAwait(false);
            
            // remove the basket id from all productitem lists
            foreach (var id in itemIds)
            {
                var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<ProductItems>(id).ConfigureAwait(false);
                item.Baskets = item.Baskets.TryRemove(e.BasketId);

                await ctx.App<Infrastructure.IUnitOfWork>().Update(id, item).ConfigureAwait(false);
            }
        }
        
        public async Task<Guid[]> Handle(ItemsUsingProduct service, IServiceContext ctx)
        {
            var productitems = await ctx.App<Infrastructure.IUnitOfWork>().Get<ProductItems>(service.ProductId).ConfigureAwait(false);

            return productitems?.Baskets ?? new Guid[] { };
        }

        public class ProductItems
        {
            public Guid ProductId { get; set; }
            public Guid[] Baskets { get; set; }
        }
    }
}
