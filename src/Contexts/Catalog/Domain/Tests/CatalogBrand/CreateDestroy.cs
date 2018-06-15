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
        public async Task ShouldDestoyBrand(
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
        public async Task ShouldNotDestroyUnknown(
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
