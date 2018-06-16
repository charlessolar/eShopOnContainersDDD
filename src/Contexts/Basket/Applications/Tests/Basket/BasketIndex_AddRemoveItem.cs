using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Basket.Basket
{
    public class BasketIndex_AddRemoveItem
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldAddItem(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.App.Plan<Catalog.Product.Models.CatalogProductIndex>(context.Id()).Exists(new Catalog.Product.Models.CatalogProductIndex
            {
                Price = 100
            });
            context.App.Plan<Models.BasketIndex>(context.Id()).Exists();

            var @event = context.Create<Entities.Item.Events.ItemAdded>(x =>
            {
                x.BasketId = context.Id();
                x.ProductId = context.Id();
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Updated(x =>
                x.TotalItems == 1 &&
                x.TotalQuantity == 1 &&
                x.SubTotal == 100
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldRemoveItem(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.App.Plan<Entities.Item.Models.BasketItemIndex>(Entities.Item.BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Entities.Item.Models.BasketItemIndex
            {
                Quantity = 1,
                ProductPrice = 100
            });
            context.App.Plan<Models.BasketIndex>(context.Id()).Exists(new Models.BasketIndex
            {
                TotalItems = 1,
                TotalQuantity = 1,
                SubTotal = 100
            });

            var @event = context.Create<Entities.Item.Events.ItemRemoved>(x =>
            {
                x.BasketId = context.Id();
                x.ProductId = context.Id();
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Updated(x =>
                x.TotalItems == 0 &&
                x.TotalQuantity == 0 &&
                x.SubTotal == 0
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldUpdateQuantity(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.App.Plan<Entities.Item.Models.BasketItemIndex>(Entities.Item.BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Entities.Item.Models.BasketItemIndex
            {
                Quantity = 1,
                ProductPrice = 100
            });
            context.App.Plan<Models.BasketIndex>(context.Id()).Exists(new Models.BasketIndex
            {
                TotalItems = 1,
                TotalQuantity = 1,
                SubTotal = 100
            });

            var @event = context.Create<Entities.Item.Events.QuantityUpdated>(x =>
            {
                x.BasketId = context.Id();
                x.ProductId = context.Id();
                x.Quantity = 2;
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Updated(x =>
                x.TotalItems == 1 &&
                x.TotalQuantity == 2 &&
                x.SubTotal == 200
            );
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldUpdatePrice(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.Processor.Plan<Services.BasketsUsingProduct, Guid[]>(new Services.BasketsUsingProduct { ProductId = context.Id() }).Response(new Guid[] { context.Id() });
            context.App.Plan<Models.BasketIndex>(context.Id()).Exists(new Models.BasketIndex
            {
                SubTotal = 100,
            });
            context.App.Plan<Entities.Item.Models.BasketItemIndex>(Entities.Item.BasketItemIndex.ItemIdGenerator(context.Id(), context.Id())).Exists(new Entities.Item.Models.BasketItemIndex
            {
                Quantity=1,
                ProductPrice = 100
            });

            var @event = context.Create<Catalog.Product.Events.PriceUpdated>(x =>
              {
                  x.ProductId = context.Id();
                  x.Price = 1;
              });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Updated(x => x.SubTotal == 1);

        }
    }
}
