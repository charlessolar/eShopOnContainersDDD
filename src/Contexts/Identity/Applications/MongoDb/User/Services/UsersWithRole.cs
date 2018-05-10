using System;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Extensions;
using NServiceBus;
using Infrastructure.Extensions;

namespace eShop.Identity.User.Services
{
    public class UsersWithRole : IService<Guid[]>
    {
        public Guid RoleId { get; set; }
    }
    public class UsersWithRoleHandler :
        IHandleMessages<Entities.Role.Events.Assigned>,
        IHandleMessages<Entities.Role.Events.Revoked>,
        IProvideService<UsersWithRole, Guid[]>
    {
        public async Task Handle(Entities.Role.Events.Assigned e, IMessageHandlerContext ctx)
        {
            var userroles = await ctx.App<Infrastructure.IUnitOfWork>().Get<UserRoles>(e.RoleId).ConfigureAwait(false);
            if (userroles == null)
            {
                userroles = new UserRoles
                {
                    RoleId = e.RoleId,
                    Users = new Guid[] { e.UserId }
                };
                await ctx.App<Infrastructure.IUnitOfWork>().Add(e.RoleId, userroles).ConfigureAwait(false);
            }
            else
            {
                userroles.Users = userroles.Users.TryAdd(e.UserId);
                await ctx.App<Infrastructure.IUnitOfWork>().Update(e.RoleId, userroles).ConfigureAwait(false);
            }
        }

        public async Task Handle(Entities.Role.Events.Revoked e, IMessageHandlerContext ctx)
        {
            var userroles = await ctx.App<Infrastructure.IUnitOfWork>().Get<UserRoles>(e.RoleId).ConfigureAwait(false);
            userroles.Users = userroles.Users.TryRemove(e.UserId);
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.RoleId, userroles).ConfigureAwait(false);
        }

        public async Task<Guid[]> Handle(UsersWithRole service, IServiceContext ctx)
        {
            var userroles = await ctx.App<Infrastructure.IUnitOfWork>().Get<UserRoles>(service.RoleId).ConfigureAwait(false);

            return userroles?.Users ?? new Guid[] { };
        }

        class UserRoles
        {
            public Guid RoleId { get; set; }
            public Guid[] Users { get; set; }
        }
    }
}
