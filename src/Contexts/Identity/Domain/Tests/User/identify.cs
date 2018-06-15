using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Security;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Identity.User
{
    public class Identify
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldUserIdentified(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<User>(context.Id())
                .HasEvent<Events.Registered>(x =>
                {
                    x.UserName = context.Id();
                    x.GivenName = "test";
                    x.Password = PasswordStorage.CreateHash("test");
                });

            var command = new Commands.Identify
            {
                UserName = context.Id(),
                Password = "test"
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<User>(context.Id()).Raised<Events.Identified>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotIdentifyUser(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<User>(context.Id())
                .HasEvent<Events.Registered>(x =>
                {
                    x.UserName = context.Id();
                    x.GivenName = "test";
                    x.Password = PasswordStorage.CreateHash("test");
                });

            var command = new Commands.Identify
            {
                UserName = context.Id(),
                Password = "test2"
            };
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldIdentifyWithNewPassword(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<User>(context.Id())
                .HasEvent<Events.Registered>(x =>
                {
                    x.UserName = context.Id();
                    x.GivenName = "test";
                    x.Password = PasswordStorage.CreateHash("test");
                })
                .HasEvent<Events.PasswordChanged>(x =>
                {
                    x.UserName = context.Id();
                    x.Password = PasswordStorage.CreateHash("test2");
                });

            var command = new Commands.Identify
            {
                UserName = context.Id(),
                Password = "test2"
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<User>(context.Id()).Raised<Events.Identified>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotIdentifyUserWithOldPassword(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<User>(context.Id())
                .HasEvent<Events.Registered>(x =>
                {
                    x.UserName = context.Id();
                    x.GivenName = "test";
                    x.Password = PasswordStorage.CreateHash("test");
                })
                .HasEvent<Events.PasswordChanged>(x =>
                {
                    x.UserName = context.Id();
                    x.Password = PasswordStorage.CreateHash("test2");
                });

            var command = new Commands.Identify
            {
                UserName = context.Id(),
                Password = "test"
            };
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotIdentifyDisabledUser(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<User>(context.Id())
                .HasEvent<Events.Registered>(x =>
                {
                    x.UserName = context.Id();
                    x.GivenName = "test";
                    x.Password = PasswordStorage.CreateHash("test");
                })
                .HasEvent<Events.Disabled>(x =>
                {
                    x.UserName = context.Id();
                });

            var command = new Commands.Identify
            {
                UserName = context.Id(),
                Password = "test"
            };
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);
        }
    }
}
