using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Extensions;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Basket.Basket.Services
{

    public class BasketsUsingProductHandler :
        IHandleMessages<Entities.Item.Events.ItemAdded>,
        IHandleMessages<Entities.Item.Events.ItemRemoved>,
        IHandleMessages<eShop.Basket.Basket.Events.Destroyed>,
        IHandleMessages<Catalog.Product.Events.Removed>,
        IProvideService<BasketsUsingProduct, Guid[]>
    {
        // Todo: note that as more and more baskets happen these lists can grow pretty big.  Obviously this is a small scale solution
        // it is possible to do queries in mongo so it wouldn't be hard to stop doing this
        public async Task Handle(Entities.Item.Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var productitems = await ctx.UoW().Get<ProductBaskets>(e.ProductId)
                .ConfigureAwait(false);
            if (productitems == null)
            {
                productitems = new ProductBaskets
                {
                    Id = e.ProductId,
                    Baskets = new[] {e.BasketId}
                };
                await ctx.UoW().Add(e.ProductId, productitems).ConfigureAwait(false);
            }
            else
            {
                productitems.Baskets = productitems.Baskets.TryAdd(e.BasketId);
                await ctx.UoW().Update(e.ProductId, productitems).ConfigureAwait(false);
            }
        }

        public async Task Handle(Entities.Item.Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            var productitems = await ctx.UoW().Get<ProductBaskets>(e.ProductId)
                .ConfigureAwait(false);

            productitems.Baskets = productitems.Baskets.TryRemove(e.BasketId);
            await ctx.UoW().Update(e.ProductId, productitems).ConfigureAwait(false);
        }

        public Task Handle(Catalog.Product.Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<ProductBaskets>(e.ProductId);
        }

        // when a basket is destroyed, remove the basket from all ProductItems
        public async Task Handle(eShop.Basket.Basket.Events.Destroyed e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Models.Basket>(e.BasketId).ConfigureAwait(false);
            
            // remove the basket id from all productitem lists
            foreach (var item in basket.Items)
            {
                var productBasket = await ctx.UoW().Get<ProductBaskets>(item.ProductId).ConfigureAwait(false);

                productBasket.Baskets = productBasket.Baskets.TryRemove(e.BasketId);

                await ctx.UoW().Update(item.ProductId, productBasket).ConfigureAwait(false);
            }
        }
        
        public async Task<Guid[]> Handle(BasketsUsingProduct service, IServiceContext ctx)
        {
            var productitems = await ctx.UoW().Get<ProductBaskets>(service.ProductId).ConfigureAwait(false);

            return productitems?.Baskets ?? new Guid[] { };
        }

        class ProductBaskets
        {
            public Guid Id { get; set; }
            public Guid[] Baskets { get; set; }
        }
    }
}
