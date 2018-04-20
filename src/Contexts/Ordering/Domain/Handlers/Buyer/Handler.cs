using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer
{
    public class Handler :
        IHandleMessages<Commands.Create>
    {
        public async Task Handle(Commands.Create command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().New(command.BuyerId).ConfigureAwait(false);
            buyer.Create(command.GivenName);
        }
    }
}
