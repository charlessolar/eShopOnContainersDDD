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
    public class BasketItemIndex_Details
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldChangeQuantity(
            TestableContext context,
            BasketItemIndex handler
            )
        {
            context.App.Plan<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Models.BasketItemIndex
            {
                Quantity = 1
            });

            var @event = context.Create<Entities.Item.Events.QuantityUpdated>(x =>
            {
                x.BasketId = context.Id();
                x.ProductId = context.Id();
                x.Quantity = 2;
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Updated(x =>
                x.Quantity == 2
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldChangeDescription(
            TestableContext context,
            BasketItemIndex handler
            )
        {
            context.Processor.Plan<eShop.Basket.Basket.Services.BasketsUsingProduct, Guid[]>(new eShop.Basket.Basket.Services.BasketsUsingProduct { ProductId = context.Id() }).Response(new Guid[] { context.Id() });
            context.App.Plan<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Models.BasketItemIndex
            {
                ProductDescription = "change"
            });

            var @event = context.Create<Catalog.Product.Events.DescriptionUpdated>(x =>
            {
                x.ProductId = context.Id();
                x.Description = "test";
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Updated(x =>
                x.ProductDescription == "test"
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldChangePicture(
            TestableContext context,
            BasketItemIndex handler
            )
        {
            context.Processor.Plan<eShop.Basket.Basket.Services.BasketsUsingProduct, Guid[]>(new eShop.Basket.Basket.Services.BasketsUsingProduct { ProductId = context.Id() }).Response(new Guid[] { context.Id() });
            context.App.Plan<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Models.BasketItemIndex
            {
                ProductPictureContents="change",
                ProductPictureContentType ="change"
            });

            var @event = context.Create<Catalog.Product.Events.PictureSet>(x =>
            {
                x.ProductId = context.Id();
                x.Content = "test";
                x.ContentType = "test";
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Updated(x =>
                x.ProductPictureContents == "test" &&
                x.ProductPictureContentType == "test"
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldChangePrice(
            TestableContext context,
            BasketItemIndex handler
            )
        {
            context.Processor.Plan<eShop.Basket.Basket.Services.BasketsUsingProduct, Guid[]>(new eShop.Basket.Basket.Services.BasketsUsingProduct { ProductId = context.Id() }).Response(new Guid[] { context.Id() });
            context.App.Plan<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Models.BasketItemIndex
            {
                ProductPrice = 1
            });

            var @event = context.Create<Catalog.Product.Events.PriceUpdated>(x =>
            {
                x.ProductId = context.Id();
                x.Price = 2;
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketItemIndex>(BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Updated(x =>
                x.ProductPrice == 2
            );
        }
    }
}
