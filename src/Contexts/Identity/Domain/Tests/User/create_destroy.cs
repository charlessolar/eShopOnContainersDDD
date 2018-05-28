using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Identity.User
{
    public class create_destroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task Should_create_user(
            TestableContext context,
            Handler handler
            )
        {
            var command = new Commands.Register
            {
                UserName = context.Id(),
                GivenName = "test",
                Password = "test"
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<User>(context.Id()).Raised<Events.Registered>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_disable_user(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<User>(context.Id())
                .HasEvent<Events.Registered>(x =>
                {
                    x.UserName = context.Id();
                    x.GivenName = "test";
                    x.Password = "test";
                });

            var command = new Commands.Disable
            {
                UserName = context.Id()
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<User>(context.Id()).Raised<Events.Disabled>();
        }
    }
}
