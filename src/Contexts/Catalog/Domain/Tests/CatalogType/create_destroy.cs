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
    public class create_destroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task Should_create_brand(
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
        public async Task Should_destroy_brand(
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
        public async Task Should_not_destroy_unknown(
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
