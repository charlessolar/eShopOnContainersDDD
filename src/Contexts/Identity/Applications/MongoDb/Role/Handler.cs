using System;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Application;
using NServiceBus;
using Infrastructure.Extensions;

namespace eShop.Identity.Role
{
    public class Handler :
        IHandleMessages<Events.Activated>,
        IHandleMessages<Events.Deactivated>,
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>,
        IHandleMessages<Events.Revoked>
    {
        public async Task Handle(Events.Activated e, IMessageHandlerContext ctx)
        {
            var role = await ctx.Uow().Get<Models.Role>(e.RoleId).ConfigureAwait(false);
            role.Disabled = false;

            await ctx.Uow().Update(e.RoleId, role).ConfigureAwait(false);
        }
        public async Task Handle(Events.Deactivated e, IMessageHandlerContext ctx)
        {
            var role = await ctx.Uow().Get<Models.Role>(e.RoleId).ConfigureAwait(false);
            role.Disabled = true;

            await ctx.Uow().Update(e.RoleId, role).ConfigureAwait(false);
        }
        public async Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.Role
            {
                Id = e.RoleId,
                Name = e.Name,
            };

            await ctx.Uow().Add(e.RoleId, model).ConfigureAwait(false);
        }
        public async Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            await ctx.Uow().Delete<Models.Role>(e.RoleId).ConfigureAwait(false);
        }
        public async Task Handle(Events.Revoked e, IMessageHandlerContext ctx)
        {
            var role = await ctx.Uow().Get<Models.Role>(e.RoleId).ConfigureAwait(false);

            var userIds = await ctx.Service<User.Services.UsersWithRole, string[]>(x => { x.RoleId = e.RoleId; })
                .ConfigureAwait(false);

            // Remove the role from all users
            foreach (var id in userIds)
            {
                var user = await ctx.Uow().Get<User.Models.User>(id).ConfigureAwait(false);
                user.Roles = user.Roles.TryRemove(role.Name);
                await ctx.Uow().Update(id, user).ConfigureAwait(false);
            }
        }
    }
}
