using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Order
{
    public class Handler :
        IHandleMessages<Commands.Draft>,
        IHandleMessages<Commands.Cancel>,
        IHandleMessages<Commands.Confirm>,
        IHandleMessages<Commands.Pay>,
        IHandleMessages<Commands.Ship>,
        IHandleMessages<Commands.ChangeAddress>,
        IHandleMessages<Commands.ChangePaymentMethod>
    {
        public async Task Handle(Commands.Draft command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer.Buyer>().Get(command.UserName).ConfigureAwait(false);
            var basket = await ctx.For<Basket.Basket.Basket>().Get(command.BasketId).ConfigureAwait(false);

            var order = await ctx.For<Order>().New(command.OrderId).ConfigureAwait(false);
            var address = await buyer.For<Buyer.Entities.Address.Address>().Get(command.AddressId).ConfigureAwait(false);
            var method = await buyer.For<Buyer.Entities.PaymentMethod.PaymentMethod>().Get(command.PaymentMethodId).ConfigureAwait(false);

            order.Draft(buyer.State, basket.State, address.State, method.State);
        }

        public async Task Handle(Commands.Cancel command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            order.Cancel();
        }
        public async Task Handle(Commands.Pay command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            order.Pay();
        }
        public async Task Handle(Commands.Ship command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            order.Ship();
        }
        public async Task Handle(Commands.Confirm command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            order.Confirm();
        }
        public async Task Handle(Commands.ChangeAddress command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var buyer = await ctx.For<Buyer.Buyer>().Get(order.State.UserName).ConfigureAwait(false);
            var address = await ctx.For<Buyer.Entities.Address.Address>().Get(command.AddressId).ConfigureAwait(false);

            order.ChangeAddress(address.State);
        }
        public async Task Handle(Commands.ChangePaymentMethod command, IMessageHandlerContext ctx)
        {
            var order = await ctx.For<Order>().Get(command.OrderId).ConfigureAwait(false);
            var buyer = await ctx.For<Buyer.Buyer>().Get(order.State.UserName).ConfigureAwait(false);
            var method = await ctx.For<Buyer.Entities.PaymentMethod.PaymentMethod>().Get(command.PaymentMethodId).ConfigureAwait(false);

            order.ChangePaymentMethod(method.State);
        }

    }
}
