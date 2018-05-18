using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod
{
    public class Handler :
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.Remove>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            var method = await ctx.For<PaymentMethod>().New(command.PaymentMethodId).ConfigureAwait(false);

            method.Add(command.Alias, command.CardNumber, command.SecurityNumber, command.CardholderName,
                command.Expiration, command.CardType);
        }

        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            var method = await ctx.For<PaymentMethod>().Get(command.PaymentMethodId).ConfigureAwait(false);

            method.Remove();
        }
    }
}
