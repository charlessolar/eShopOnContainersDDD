﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Domain;
using NServiceBus;


namespace eShop.Ordering.Order.Entities.Item
{
    public class Handler : 
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.OverridePrice>,
        IHandleMessages<Commands.Remove>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var item = await ctx.For<Item>().New(command.ProductId).ConfigureAwait(false);

            item.Add(command.Quantity);
        }

        public async Task Handle(Commands.OverridePrice command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var item = await ctx.For<Item>().Get(command.ProductId).ConfigureAwait(false);

            item.OverridePrice(command.Price);
        }
        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var item = await ctx.For<Item>().Get(command.ProductId).ConfigureAwait(false);

            item.Remove();
        }
    }
}
