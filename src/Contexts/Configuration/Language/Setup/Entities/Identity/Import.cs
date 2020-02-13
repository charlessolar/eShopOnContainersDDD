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

            var saga = ctx.Saga(Guid.NewGuid());
            // define all used roles
            foreach (var role in Users.SelectMany(x => x.Roles).Distinct())
            {
                var id = Guid.NewGuid();
                roleIds[role] = id;

                saga.Command(new eShop.Identity.Role.Commands.Define
                {
                    RoleId = id,
                    Name = role
                });
            }

            // define all defined users
            foreach (var user in Users)
            {
                saga.Command(new eShop.Identity.User.Commands.Register
                {
                    GivenName = user.GivenName,
                    UserName = user.UserName,
                    Password = user.Password
                });

                foreach (var role in user.Roles)
                {
                    saga.Command(new eShop.Identity.User.Entities.Role.Commands.Assign
                    {
                        UserName = user.UserName,
                        RoleId = roleIds[role]
                    });
                }
            }
            await saga.Start().ConfigureAwait(false);

            // Define some random users
            var bogus = new Faker<Types.User>()
                .StrictMode(false)
                .Rules((f, o) =>
                {
                    o.UserName = f.Internet.UserName();
                    o.Password = f.Internet.Password();
                    o.GivenName = f.Name.FindName(withPrefix: false, withSuffix: false);
                    o.Roles = new[] { "customer" };
                });

            var users = bogus.Generate(20);
            var saga2 = ctx.Saga(Guid.NewGuid());
            foreach (var user in users)
            {
                saga2.Command(new eShop.Identity.User.Commands.Register
                {
                    GivenName = user.GivenName,
                    UserName = user.UserName,
                    Password = user.Password
                });

                foreach (var role in user.Roles)
                {
                    saga2.Command(new eShop.Identity.User.Entities.Role.Commands.Assign
                    {
                        UserName = user.UserName,
                        RoleId = roleIds[role]
                    });
                }
            }
            await saga2.Start().ConfigureAwait(false);

            Users = Users.Concat(users).ToArray();
        }
    }
}
