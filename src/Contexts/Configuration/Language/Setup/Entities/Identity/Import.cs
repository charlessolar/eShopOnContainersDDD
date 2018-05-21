using Infrastructure.Extensions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Identity
{
    public class Import
    {
        public static async Task Seed(IMessageHandlerContext ctx)
        {

            var roleId1 = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();

            await ctx.CommandToDomain(new eShop.Identity.User.Commands.Register
            {
                GivenName = "administrator",
                UserName = "administrator",
                Password = "12345678"
            }).ConfigureAwait(false);

            await ctx.CommandToDomain(new eShop.Identity.Role.Commands.Define
            {
                RoleId = roleId1,
                Name = "administrator"
            }).ConfigureAwait(false);
            await ctx.CommandToDomain(new eShop.Identity.Role.Commands.Define
            {
                RoleId = roleId2,
                Name = "customer"
            }).ConfigureAwait(false);

            await ctx.CommandToDomain(new eShop.Identity.User.Entities.Role.Commands.Assign
            {
                UserName = "administrator",
                RoleId = roleId1
            }).ConfigureAwait(false);
            await ctx.CommandToDomain(new eShop.Identity.User.Entities.Role.Commands.Assign
            {
                UserName = "administrator",
                RoleId = roleId2
            }).ConfigureAwait(false);
        }
    }
}
