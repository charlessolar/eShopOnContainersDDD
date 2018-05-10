using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;

namespace eShop.Identity.User
{
    public class Handler :
        IHandleMessages<Commands.Register>,
        IHandleMessages<Commands.Enable>,
        IHandleMessages<Commands.Disable>
    {
        public async Task Handle(Commands.Register command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().New(command.UserId).ConfigureAwait(false);
            user.Register(command.GivenName);
        }

        public async Task Handle(Commands.Enable command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserId).ConfigureAwait(false);
            user.Enable();
        }

        public async Task Handle(Commands.Disable command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserId).ConfigureAwait(false);
            user.Disable();
        }
    }
}
