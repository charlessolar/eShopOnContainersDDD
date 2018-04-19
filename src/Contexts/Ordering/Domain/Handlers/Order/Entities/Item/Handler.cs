using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;


namespace eShop.Ordering.Order.Entities.Item
{
    public class Handler : 
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.ChangeQuantity>,
        IHandleMessages<Commands.Remove>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var item = await ctx.For<Item>().New(command.ItemId).ConfigureAwait(false);

            var product = await ctx.For<Catalog.Product.Product>().Get(command.ProductId).ConfigureAwait(false);

            item.Add(product.State, command.Quantity);
        }

        public async Task Handle(Commands.ChangeQuantity command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var item = await ctx.For<Item>().Get(command.ItemId).ConfigureAwait(false);

            item.ChangeQuantity(command.Quantity);
        }
        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var item = await ctx.For<Item>().Get(command.ItemId).ConfigureAwait(false);

            item.Remove();
        }
    }
}
