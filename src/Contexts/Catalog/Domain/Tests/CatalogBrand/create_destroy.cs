using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Catalog.CatalogBrand
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
                BrandId = context.Id(),
                Brand = "test"
            }, context).ConfigureAwait(false);

            context.UoW.Check<Brand>(context.Id()).Raised<Events.Defined>(x =>
            {
                x.BrandId = context.Id();
                x.Brand = "test";
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_destroy_brand(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Brand>(context.Id()).HasEvent<Events.Defined>(x =>
            {
                x.BrandId = context.Id();
                x.Brand = "test";
            });

            await handler.Handle(new Commands.Destroy
            {
                BrandId = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Brand>(context.Id()).Raised<Events.Destroyed>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_not_destroy_unknown(
            TestableContext context,
            Handler handler
            )
        {            
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new Commands.Destroy
            {
                BrandId = context.Id()
            }, context)).ConfigureAwait(false);

        }
    }
}
