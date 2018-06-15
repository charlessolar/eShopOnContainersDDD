using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Identity.Role
{
    public class Activation
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldActivate(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                })
                .HasEvent<Events.Deactivated>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Activate
            {
                RoleId = context.Id()
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<Role>(context.Id()).Raised<Events.Activated>();

        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldInitialAlreadyActive(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Activate
            {
                RoleId = context.Id()
            };

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);

        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotActivateAlreadyActivated(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                })
                .HasEvent<Events.Activated>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Activate
            {
                RoleId = context.Id()
            };

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);

        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotActivateDestroyed(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                })
                .HasEvent<Events.Destroyed>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Activate
            {
                RoleId = context.Id()
            };

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);

        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldDeactivate(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Deactivate
            {
                RoleId = context.Id()
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<Role>(context.Id()).Raised<Events.Deactivated>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotDeactivateDeactivated(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                })
                .HasEvent<Events.Deactivated>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Deactivate
            {
                RoleId = context.Id()
            };
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotDeactivateDestroyed(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Role>(context.Id())
                .HasEvent<Events.Defined>(x =>
                {
                    x.RoleId = context.Id();
                })
                .HasEvent<Events.Destroyed>(x =>
                {
                    x.RoleId = context.Id();
                });

            var command = new Commands.Deactivate
            {
                RoleId = context.Id()
            };
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);
        }
    }
}
