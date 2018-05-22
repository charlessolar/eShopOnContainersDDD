using Aggregates;
using Bogus;
using Infrastructure.Extensions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Identity
{
    public class Import
    {
        public static Types.User[] Users = new[]
        {
            new Types.User {UserName = "administrator", GivenName="Administrator", Password="12345678", Roles= new[]{"administrator", "customer" }}
        };

        public static async Task Seed(IMessageHandlerContext ctx)
        {
            var roleIds = new Dictionary<string, Guid>();
            
            await ctx.LocalSaga(async bus =>
            {
                // define all used roles
                foreach (var role in Users.SelectMany(x => x.Roles).Distinct())
                {
                    var id = Guid.NewGuid();
                    roleIds[role] = id;
                    
                    await bus.CommandToDomain(new eShop.Identity.Role.Commands.Define
                    {
                        RoleId = id,
                        Name = role
                    }).ConfigureAwait(false);
                }

                // define all defined users
                foreach (var user in Users)
                {
                    await bus.CommandToDomain(new eShop.Identity.User.Commands.Register
                    {
                        GivenName = user.GivenName,
                        UserName = user.UserName,
                        Password = user.Password
                    }).ConfigureAwait(false);

                    foreach(var role in user.Roles)
                    {
                        await bus.CommandToDomain(new eShop.Identity.User.Entities.Role.Commands.Assign
                        {
                            UserName = user.UserName,
                            RoleId = roleIds[role]
                        }).ConfigureAwait(false);
                    }
                }
            }).ConfigureAwait(false);

            // Define some random users
            var bogus = new Faker<Types.User>()
                .StrictMode(false)
                .Rules((f, o) =>
                {
                    o.UserName = f.Internet.UserName();
                    o.Password = f.Internet.Password();
                    o.GivenName = f.Name.FindName();
                });

            var users = bogus.Generate(10);
            await ctx.LocalSaga(async bus =>
            {
                foreach(var user in users)
                {
                    await bus.CommandToDomain(new eShop.Identity.User.Commands.Register
                    {
                        GivenName = user.GivenName,
                        UserName = user.UserName,
                        Password = user.Password
                    }).ConfigureAwait(false);

                    await bus.CommandToDomain(new eShop.Identity.User.Entities.Role.Commands.Assign
                    {
                        UserName = user.UserName,
                        RoleId = roleIds["customer"]
                    }).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);

            Users = Users.Concat(users).ToArray();
        }
    }
}
