using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Identity.User.Entities.Role
{
    public class Handler :
        IHandleMessages<Commands.Assign>,
        IHandleMessages<Commands.Revoke>
    {

        public async Task Handle(Commands.Assign command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserId).ConfigureAwait(false);
            var role = await user.For<Role>().TryGet(command.RoleId).ConfigureAwait(false);
            // Allows a role to already exist (was previously revoked)
            if (role == null)
            {
                role = await user.For<Role>().New(command.RoleId).ConfigureAwait(false);
            }
            role.Assign();
        }
        public async Task Handle(Commands.Revoke command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserId).ConfigureAwait(false);
            var role = await user.For<Role>().Get(command.RoleId).ConfigureAwait(false);
            role.Revoke();
        }
    }
}
