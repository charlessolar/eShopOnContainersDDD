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
    public class reordering
    {
        [Theory, AutoFakeItEasyData]
        public async Task Should_mark_reorder(
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

            await handler.Handle(new Commands.MarkReordered
            {
                ProductId = context.Id(),
            }, context).ConfigureAwait(false);

            context.UoW.Check<Product>(context.Id()).Raised<Events.ReorderMarked>(x =>
            {
                x.ProductId = context.Id();
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_unmark_reorder(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Product>(context.Id())
                .HasEvent<Events.Added>(x =>
                {
                    x.ProductId = context.Id();
                    x.CatalogBrandId = context.Id();
                    x.CatalogTypeId = context.Id();
                    x.Name = "test";
                    x.Price = 1;
                })
                .HasEvent<Events.ReorderMarked>(x =>
                {
                    x.ProductId = context.Id();
                });

            await handler.Handle(new Commands.UnMarkReordered
            {
                ProductId = context.Id(),
            }, context).ConfigureAwait(false);

            context.UoW.Check<Product>(context.Id()).Raised<Events.ReorderUnMarked>(x =>
            {
                x.ProductId = context.Id();
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_not_double_mark_reorder(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Product>(context.Id())
                .HasEvent<Events.Added>(x =>
                {
                    x.ProductId = context.Id();
                    x.CatalogBrandId = context.Id();
                    x.CatalogTypeId = context.Id();
                    x.Name = "test";
                    x.Price = 1;
                })
                .HasEvent<Events.ReorderMarked>(x =>
                {
                    x.ProductId = context.Id();
                });
            
            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(new Commands.MarkReordered
            {
                ProductId = context.Id()
            }, context)).ConfigureAwait(false);

        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_not_unmark_reorder(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Product>(context.Id())
                .HasEvent<Events.Added>(x =>
                {
                    x.ProductId = context.Id();
                    x.CatalogBrandId = context.Id();
                    x.CatalogTypeId = context.Id();
                    x.Name = "test";
                    x.Price = 1;
                });

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(new Commands.UnMarkReordered
            {
                ProductId = context.Id()
            }, context)).ConfigureAwait(false);

        }
    }
}
