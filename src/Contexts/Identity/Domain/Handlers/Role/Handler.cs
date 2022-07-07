using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Domain;
using NServiceBus;

namespace eShop.Identity.Role
{
    public class Handler :
        IHandleMessages<Commands.Activate>,
        IHandleMessages<Commands.Deactivate>,
        IHandleMessages<Commands.Define>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.Revoke>
    {
        public async Task Handle(Commands.Activate command, IMessageHandlerContext ctx)
        {
            var role = await ctx.For<Role>().Get(command.RoleId).ConfigureAwait(false);
            role.Activate();
        }
        public async Task Handle(Commands.Deactivate command, IMessageHandlerContext ctx)
        {
            var role = await ctx.For<Role>().Get(command.RoleId).ConfigureAwait(false);
            role.Deactivate();
        }
        public async Task Handle(Commands.Define command, IMessageHandlerContext ctx)
        {
            var role = await ctx.For<Role>().New(command.RoleId).ConfigureAwait(false);
            role.Define(command.Name);
        }
        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var role = await ctx.For<Role>().Get(command.RoleId).ConfigureAwait(false);
            role.Destroy();
        }
        public async Task Handle(Commands.Revoke command, IMessageHandlerContext ctx)
        {
            var role = await ctx.For<Role>().Get(command.RoleId).ConfigureAwait(false);
            role.Revoke();
        }
    }
}
