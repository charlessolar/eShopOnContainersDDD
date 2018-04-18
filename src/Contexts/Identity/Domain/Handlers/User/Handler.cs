using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;

namespace eShop.Identity.User
{
    public class Handler :
        IHandleMessages<Commands.Register>
    {
        public async Task Handle(Commands.Register command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().New(command.UserId).ConfigureAwait(false);
            user.Register(command.GivenName);
        }
    }
}
