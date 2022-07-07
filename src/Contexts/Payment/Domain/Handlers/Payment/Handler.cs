using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Domain;
using NServiceBus;

namespace eShop.Payment.Payment
{
    public class Handler : 
        IHandleMessages<Commands.Charge>,
        IHandleMessages<Commands.Settle>,
        IHandleMessages<Commands.Cancel>
    {
        public async Task Handle(Commands.Charge command, IMessageHandlerContext ctx)
        {
            var payment = await ctx.For<Payment>().New(command.PaymentId).ConfigureAwait(false);
            var buyer = await ctx.For<Ordering.Buyer.Buyer>().Get(command.UserName).ConfigureAwait(false);
            var order = await ctx.For<Ordering.Order.Order>().Get(command.OrderId).ConfigureAwait(false);
            var method = await buyer.For<Ordering.Buyer.Entities.PaymentMethod.PaymentMethod>().Get(command.PaymentMethodId).ConfigureAwait(false);

            payment.Charge(buyer.State, order.State, method.State);
        }
        public async Task Handle(Commands.Settle command, IMessageHandlerContext ctx)
        {
            var payment = await ctx.For<Payment>().Get(command.PaymentId).ConfigureAwait(false);
            payment.Settle();
        }
        public async Task Handle(Commands.Cancel command, IMessageHandlerContext ctx)
        {
            var payment = await ctx.For<Payment>().Get(command.PaymentId).ConfigureAwait(false);
            payment.Cancel();
        }
    }
}
