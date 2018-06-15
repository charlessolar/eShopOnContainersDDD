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
    public class CreateDestroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldCreateRole(
            TestableContext context,
            Handler handler
            )
        {
            var command = new Commands.Define
            {
                RoleId = context.Id()
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<Role>(context.Id()).Raised<Events.Defined>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldDestroyRole(
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

            var command = new Commands.Destroy
            {
                RoleId = context.Id()
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<Role>(context.Id()).Raised<Events.Destroyed>(x =>
            {
                x.RoleId = context.Id();
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotDestroyUnknown(
            TestableContext context,
            Handler handler
            )
        {
            var command = new Commands.Destroy
            {
                RoleId = context.Id()
            };
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, context)).ConfigureAwait(false);
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotDestroyActiveRole(
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

            var command = new Commands.Destroy
            {
                RoleId = context.Id()
            };
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(command, context)).ConfigureAwait(false);
            
        }
    }
}
