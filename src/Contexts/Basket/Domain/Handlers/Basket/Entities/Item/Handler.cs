using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Aggregates;

namespace eShop.Basket.Basket.Entities.Item
{
    public class Handler :
        IHandleMessages<Commands.AddItem>,
        IHandleMessages<Commands.RemoveItem>,
        IHandleMessages<Commands.UpdateQuantity>
    {
        public async Task Handle(Commands.AddItem command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().Get(command.BasketId).ConfigureAwait(false);
            var item = await basket.For<Item>().New(command.ProductId).ConfigureAwait(false);

            var product = await ctx.For<Catalog.Product.Product>().Get(command.ProductId).ConfigureAwait(false);

            item.Add();
        }
        public async Task Handle(Commands.RemoveItem command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().Get(command.BasketId).ConfigureAwait(false);
            var item = await basket.For<Item>().Get(command.ProductId).ConfigureAwait(false);

            item.Remove();
        }
        public async Task Handle(Commands.UpdateQuantity command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().Get(command.BasketId).ConfigureAwait(false);
            var item = await basket.For<Item>().Get(command.ProductId).ConfigureAwait(false);

            item.UpdateQuantity(command.Quantity);
        }
    }
}
