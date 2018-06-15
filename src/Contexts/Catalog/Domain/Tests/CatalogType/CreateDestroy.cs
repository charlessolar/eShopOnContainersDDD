using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace eShop.Catalog.CatalogType
{
    public class CreateDestroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldCreateBrand(
            TestableContext context,
            Handler handler
            )
        {
            await handler.Handle(new Commands.Define
            {
                TypeId = context.Id(),
                Type = "test"
            }, context).ConfigureAwait(false);

            context.UoW.Check<Type>(context.Id()).Raised<Events.Defined>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldDestroyBrand(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Type>(context.Id()).HasEvent<Events.Defined>(x =>
            {
                x.TypeId = context.Id();
                x.Type = "test";
            });

            await handler.Handle(new Commands.Destroy
            {
                TypeId = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Type>(context.Id()).Raised<Events.Destroyed>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotdestroyUnknown(
            TestableContext context,
            Handler handler
            )
        {
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new Commands.Destroy
            {
                TypeId = context.Id()
            }, context)).ConfigureAwait(false);

        }
    }
}
