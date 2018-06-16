using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;
using Infrastructure.Extensions;
using Infrastructure.Queries;

namespace eShop.Identity.User
{
    public class Handler :
        IHandleQueries<Queries.Identity>,
        IHandleMessages<Events.Registered>,
        IHandleMessages<Events.NameChanged>,
        IHandleMessages<Events.Disabled>,
        IHandleMessages<Events.Enabled>,
        IHandleMessages<Entities.Role.Events.Assigned>,
        IHandleMessages<Entities.Role.Events.Revoked>
    {
        public async Task Handle(Queries.Identity query, IMessageHandlerContext ctx)
        {
            var model = await ctx.UoW().Get<Models.User>(query.UserName)
                .ConfigureAwait(false);

            await ctx.Result(model).ConfigureAwait(false);
        }
        public Task Handle(Events.Registered e, IMessageHandlerContext ctx)
        {
            var model = new Models.User
            {
                Id = e.UserName,
                GivenName = e.GivenName
            };

            return ctx.UoW().Add(e.UserName, model);
        }

        public async Task Handle(Events.Disabled e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().Get<Models.User>(e.UserName).ConfigureAwait(false);
            user.Disabled = true;

            await ctx.UoW().Update(user.Id, user).ConfigureAwait(false);
        }
        public async Task Handle(Events.NameChanged e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().Get<Models.User>(e.UserName).ConfigureAwait(false);
            user.GivenName = e.GivenName;

            await ctx.UoW().Update(user.Id, user).ConfigureAwait(false);
        }

        public async Task Handle(Events.Enabled e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().Get<Models.User>(e.UserName).ConfigureAwait(false);
            user.Disabled = false;

            await ctx.UoW().Update(user.Id, user).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Role.Events.Assigned e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().Get<Models.User>(e.UserName).ConfigureAwait(false);
            var role = await ctx.UoW().Get<Role.Models.Role>(e.RoleId).ConfigureAwait(false);
            user.Roles = user.Roles.TryAdd(role.Name);

            await ctx.UoW().Update(user.Id, user).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Role.Events.Revoked e, IMessageHandlerContext ctx)
        {
            var user = await ctx.UoW().Get<Models.User>(e.UserName).ConfigureAwait(false);
            var role = await ctx.UoW().Get<Role.Models.Role>(e.RoleId).ConfigureAwait(false);
            user.Roles = user.Roles.TryRemove(role.Name);

            await ctx.UoW().Update(user.Id, user).ConfigureAwait(false);
        }
    }
}
