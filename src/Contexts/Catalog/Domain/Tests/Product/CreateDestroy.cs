using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Catalog.Product
{
    public class CreateDestroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldCreateProduct(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<CatalogBrand.Brand>(context.Id()).Exists();
            context.UoW.Plan<CatalogType.Type>(context.Id()).Exists();

            var command = new Commands.Add
            {
                ProductId = context.Id("product"),
                CatalogBrandId = context.Id(),
                CatalogTypeId = context.Id(),
                Name = "test",
                Price = 1
            };
            await handler.Handle(command, context).ConfigureAwait(false);

            context.UoW.Check<Product>(context.Id("product")).Raised<Events.Added>(x =>
            {
                x.ProductId = context.Id("product");
                x.CatalogBrandId = context.Id();
                x.CatalogTypeId = context.Id();
                x.Name = "test";
                x.Price = 1;
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldDestroyProduct(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Product>(context.Id()).HasEvent<Events.Added>(x =>
            {
                x.ProductId = context.Id();
                x.CatalogBrandId = context.Id();
                x.CatalogTypeId = context.Id();
                x.Name = "test";
                x.Price = 1;
            });

            await handler.Handle(new Commands.Remove
            {
                ProductId = context.Id(),
            }, context).ConfigureAwait(false);

            context.UoW.Check<Product>(context.Id()).Raised<Events.Removed>(x =>
            {
                x.ProductId = context.Id();
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotDestroyUnknown(
            TestableContext context,
            Handler handler
            )
        {
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new Commands.Remove
            {
                ProductId = context.Id()
            }, context)).ConfigureAwait(false);

        }
    }
}
