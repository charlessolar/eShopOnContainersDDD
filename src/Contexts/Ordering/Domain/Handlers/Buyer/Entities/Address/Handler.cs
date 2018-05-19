using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer.Entities.Address
{
    public class Handler :
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.Remove>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            var address = await ctx.For<Address>().New(command.AddressId).ConfigureAwait(false);

            address.Add(command.Alias, command.Street, command.City, command.State, command.Country, command.ZipCode);
        }

        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.For<Buyer>().Get(command.UserName).ConfigureAwait(false);
            var address = await ctx.For<Address>().Get(command.AddressId).ConfigureAwait(false);

            address.Remove();
        }
    }
}
