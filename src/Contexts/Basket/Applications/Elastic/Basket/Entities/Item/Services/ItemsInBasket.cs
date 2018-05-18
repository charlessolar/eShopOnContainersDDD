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
    public class ItemsInBasketHandler :
        IHandleMessages<Item.Events.ItemAdded>,
        IHandleMessages<Item.Events.ItemRemoved>,
        IHandleMessages<Basket.Events.Destroyed>,
        IProvideService<ItemsInBasket, Guid[]>
    {
        public async Task Handle(Item.Events.ItemAdded e, IMessageHandlerContext ctx)
        {
            var basketitems = await ctx.App<Infrastructure.IUnitOfWork>().Get<ItemsBasket>(e.BasketId)
                .ConfigureAwait(false);
            if (basketitems == null)
            {
                basketitems = new ItemsBasket
                {
                    BasketId = e.BasketId,
                    Items = new[] { e.ProductId }
                };
                await ctx.App<Infrastructure.IUnitOfWork>().Add(e.BasketId, basketitems).ConfigureAwait(false);
            }
            else
            {
                basketitems.Items = basketitems.Items.TryAdd(e.ProductId);
                await ctx.App<Infrastructure.IUnitOfWork>().Update(e.BasketId, basketitems).ConfigureAwait(false);
            }
        }

        public async Task Handle(Item.Events.ItemRemoved e, IMessageHandlerContext ctx)
        {
            var basketitems = await ctx.App<Infrastructure.IUnitOfWork>().Get<ItemsBasket>(e.BasketId)
                .ConfigureAwait(false);

            basketitems.Items = basketitems.Items.TryRemove(e.ProductId);
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.BasketId, basketitems).ConfigureAwait(false);
        }

        public Task Handle(Basket.Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<ItemsBasket>(e.BasketId);
        }
        public async Task<Guid[]> Handle(ItemsInBasket service, IServiceContext ctx)
        {
            var basketitems = await ctx.App<Infrastructure.IUnitOfWork>().Get<ItemsBasket>(service.BasketId).ConfigureAwait(false);

            return basketitems?.Items ?? new Guid[] { };
        }

        public class ItemsBasket
        {
            public Guid BasketId { get; set; }
            public Guid[] Items { get; set; }
        }
    }
}
