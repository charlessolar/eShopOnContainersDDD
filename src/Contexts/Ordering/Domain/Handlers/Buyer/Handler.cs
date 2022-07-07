using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Domain;
using NServiceBus;

namespace eShop.Ordering.Buyer
{
    public class Handler :
        IHandleMessages<Commands.Initiate>,
        IHandleMessages<Commands.MarkGoodStanding>,
        IHandleMessages<Commands.MarkSuspended>,
        IHandleMessages<Commands.SetPreferredAddress>,
        IHandleMessages<Commands.SetPreferredPaymentMethod>
    {
        public async Task Handle(Commands.Initiate command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().New(command.UserName).ConfigureAwait(false);
            buyer.Initiate(command.GivenName);
        }

        public async Task Handle(Commands.MarkGoodStanding command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            buyer.MarkGoodStanding();
        }

        public async Task Handle(Commands.MarkSuspended command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            buyer.MarkSuspended();
        }
        public async Task Handle(Commands.SetPreferredAddress command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            var address = await buyer.For<Entities.Address.Address>().Get(command.AddressId).ConfigureAwait(false);
            buyer.SetPreferredAddress(address.State);
        }
        public async Task Handle(Commands.SetPreferredPaymentMethod command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            var method = await buyer.For<Entities.PaymentMethod.PaymentMethod>().Get(command.PaymentMethodId).ConfigureAwait(false);
            buyer.SetPreferredPaymentMethod(method.State);
        }
    }
}
