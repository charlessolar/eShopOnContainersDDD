using System;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Extensions;
using NServiceBus;
using Infrastructure.Extensions;

namespace eShop.Identity.User.Services
{
    public class UsersWithRole : IService<string[]>
    {
        public Guid RoleId { get; set; }
    }
    public class UsersWithRoleHandler :
        IHandleMessages<Entities.Role.Events.Assigned>,
        IHandleMessages<Entities.Role.Events.Revoked>,
        IProvideService<UsersWithRole, string[]>
    {
        public async Task Handle(Entities.Role.Events.Assigned e, IMessageHandlerContext ctx)
        {
            var userroles = await ctx.UoW().TryGet<UserRoles>(e.RoleId).ConfigureAwait(false);
            if (userroles == null)
            {
                userroles = new UserRoles
                {
                    RoleId = e.RoleId,
                    Users = new string[] { e.UserName }
                };
                await ctx.UoW().Add(e.RoleId, userroles).ConfigureAwait(false);
            }
            else
            {
                userroles.Users = userroles.Users.TryAdd(e.UserName);
                await ctx.UoW().Update(e.RoleId, userroles).ConfigureAwait(false);
            }
        }

        public async Task Handle(Entities.Role.Events.Revoked e, IMessageHandlerContext ctx)
        {
            var userroles = await ctx.UoW().Get<UserRoles>(e.RoleId).ConfigureAwait(false);
            userroles.Users = userroles.Users.TryRemove(e.UserName);
            await ctx.UoW().Update(e.RoleId, userroles).ConfigureAwait(false);
        }

        public async Task<string[]> Handle(UsersWithRole service, IServiceContext ctx)
        {
            var userroles = await ctx.App().TryGet<UserRoles>(service.RoleId).ConfigureAwait(false);

            return userroles?.Users ?? new string[] { };
        }

            class UserRoles
            {
                public Guid RoleId { get; set; }
                public string[] Users { get; set; }
            }
    }
}
