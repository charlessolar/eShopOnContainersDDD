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
    public class AddRemove
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldAddItem(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Basket>(context.Id())
                .HasEvent<eShop.Basket.Basket.Events.Initiated>(x =>
                {
                    x.BasketId = context.Id();
                });
            context.UoW.Plan<Catalog.Product.Product>(context.Id()).Exists();

            await handler.Handle(new Commands.AddItem
            {
                BasketId = context.Id(),
                ProductId = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Basket>(context.Id()).Check<Item>(context.Id()).Raised<Events.ItemAdded>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldRemoveItem(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Basket>(context.Id())
                .HasEvent<eShop.Basket.Basket.Events.Initiated>(x =>
                {
                    x.BasketId = context.Id();
                })
                .Plan<Item>(context.Id())
                    .HasEvent<Events.ItemAdded>(x =>
                    {
                        x.BasketId = context.Id();
                        x.ProductId = context.Id();
                    });

            await handler.Handle(new Commands.RemoveItem
            {
                BasketId = context.Id(),
                ProductId = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Basket>(context.Id()).Check<Item>(context.Id()).Raised<Events.ItemRemoved>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldReAddItem(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Catalog.Product.Product>(context.Id()).Exists();
            context.UoW.Plan<Basket>(context.Id())
                .HasEvent<eShop.Basket.Basket.Events.Initiated>(x =>
                {
                    x.BasketId = context.Id();
                })
                .Plan<Item>(context.Id())
                    .HasEvent<Events.ItemAdded>(x =>
                    {
                        x.BasketId = context.Id();
                        x.ProductId = context.Id();
                    })
                    .HasEvent<Events.ItemRemoved>(x =>
                    {
                        x.BasketId = context.Id();
                        x.ProductId = context.Id();
                    });

            await handler.Handle(new Commands.AddItem
            {
                BasketId = context.Id(),
                ProductId = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Basket>(context.Id()).Check<Item>(context.Id()).Raised<Events.ItemAdded>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldNotRemoveUnknownItem(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Basket>(context.Id())
                .HasEvent<eShop.Basket.Basket.Events.Initiated>(x =>
                {
                    x.BasketId = context.Id();
                });

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new Commands.RemoveItem
            {
                BasketId = context.Id(),
                ProductId = context.Id()
            }, context)).ConfigureAwait(false);
        }
    }
}
