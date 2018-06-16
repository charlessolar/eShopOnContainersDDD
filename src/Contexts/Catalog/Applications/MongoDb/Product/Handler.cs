using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Catalog.Product
{
    public class Handler :
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.DescriptionUpdated>,
        IHandleMessages<Events.PictureSet>,
        IHandleMessages<Events.PriceUpdated>,
        IHandleMessages<Events.Removed>
    {

        public async Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var brand = await ctx.UoW()
                .Get<CatalogBrand.Models.CatalogBrand>(e.CatalogBrandId).ConfigureAwait(false);
            var type = await ctx.UoW()
                .Get<CatalogType.Models.CatalogType>(e.CatalogTypeId).ConfigureAwait(false);

            var model = new Models.CatalogProduct
            {
                Id = e.ProductId,
                Name = e.Name,
                Price = e.Price,
                CatalogBrand = brand.Brand,
                CatalogBrandId = brand.Id,
                CatalogType = type.Type,
                CatalogTypeId = type.Id
            };

            await ctx.UoW().Add(e.ProductId, model).ConfigureAwait(false);
        }
        public async Task Handle(Events.DescriptionUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.UoW().Get<Models.CatalogProduct>(e.ProductId).ConfigureAwait(false);

            product.Description = e.Description;

            await ctx.UoW().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.PictureSet e, IMessageHandlerContext ctx)
        {
            var product = await ctx.UoW().Get<Models.CatalogProduct>(e.ProductId).ConfigureAwait(false);

            product.PictureContents = e.Content;
            product.PictureContentType = e.ContentType;

            await ctx.UoW().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.PriceUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.UoW().Get<Models.CatalogProduct>(e.ProductId).ConfigureAwait(false);

            product.Price = e.Price;

            await ctx.UoW().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.CatalogProduct>(e.ProductId);
        }
    }
}
