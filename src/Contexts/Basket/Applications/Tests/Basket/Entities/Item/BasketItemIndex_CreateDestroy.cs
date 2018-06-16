using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Basket.Basket.Entities.Item
{
    public class BasketItemIndex_CreateDestroy
    {

        [Theory, AutoFakeItEasyData]
        public async Task ShouldAddItem(
            TestableContext context,
            BasketItemIndex handler
            )
        {
            context.App.Plan<Catalog.Product.Models.CatalogProductIndex>(context.Id()).Exists(new Catalog.Product.Models.CatalogProductIndex
            {
                Id = context.Id(),
                Name="test",
                Description = "test",
                PictureContents="test",
                PictureContentType = "test",
                Price = 100
            });

            var @event = context.Create<Entities.Item.Events.ItemAdded>(x =>
            {
                x.BasketId = context.Id();
                x.ProductId = context.Id();
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Added(x =>
                x.ProductPrice == 100 &&
                x.ProductName == "test" &&
                x.ProductDescription == "test"
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldRemoveItem(
            TestableContext context,
            BasketItemIndex handler
            )
        {
            context.App.Plan<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists();

            var @event = context.Create<Entities.Item.Events.ItemRemoved>(x =>
            {
                x.BasketId = context.Id();
                x.ProductId = context.Id();
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Deleted();
        }
    }
}
