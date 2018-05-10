using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;
using Infrastructure.Extensions;

namespace eShop.Identity.User
{
    public class Handler :
        IHandleMessages<Events.Registered>,
        IHandleMessages<Events.Disabled>,
        IHandleMessages<Events.Enabled>,
        IHandleMessages<Entities.Role.Events.Assigned>,
        IHandleMessages<Entities.Role.Events.Revoked>
    {
        public Task Handle(Events.Registered e, IMessageHandlerContext ctx)
        {
            var model = new Models.User
            {
                Id = e.UserName,
                GivenName = e.GivenName
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.UserName, model);
        }

        public async Task Handle(Events.Disabled e, IMessageHandlerContext ctx)
        {
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.User>(e.UserName).ConfigureAwait(false);
            user.Disabled = true;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(user.Id, user).ConfigureAwait(false);
        }

        public async Task Handle(Events.Enabled e, IMessageHandlerContext ctx)
        {
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.User>(e.UserName).ConfigureAwait(false);
            user.Disabled = false;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(user.Id, user).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Role.Events.Assigned e, IMessageHandlerContext ctx)
        {
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.User>(e.UserName).ConfigureAwait(false);
            var role = await ctx.App<Infrastructure.IUnitOfWork>().Get<Role.Models.Role>(e.RoleId).ConfigureAwait(false);
            user.Roles = user.Roles.TryAdd(role.Name);

            await ctx.App<Infrastructure.IUnitOfWork>().Update(user.Id, user).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Role.Events.Revoked e, IMessageHandlerContext ctx)
        {
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.User>(e.UserName).ConfigureAwait(false);
            var role = await ctx.App<Infrastructure.IUnitOfWork>().Get<Role.Models.Role>(e.RoleId).ConfigureAwait(false);
            user.Roles = user.Roles.TryRemove(role.Name);

            await ctx.App<Infrastructure.IUnitOfWork>().Update(user.Id, user).ConfigureAwait(false);
        }
    }
}
