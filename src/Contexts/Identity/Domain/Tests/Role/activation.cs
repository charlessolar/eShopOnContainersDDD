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
    public class activation
    {
        [Theory, AutoFakeItEasyData]
        public async Task Should_activate(
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
        public async Task Should_initial_already_active(
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
        public async Task Should_not_activate_already_activated(
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
        public async Task Should_not_activate_destroyed(
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
        public async Task Should_deactivate(
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
        public async Task Should_not_deactivate_deactivated(
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
        public async Task Should_not_deactivate_destroyed(
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
