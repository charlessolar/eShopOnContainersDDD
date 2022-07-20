using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Application;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.Product
{
    public class Handler :
        IHandleQueries<Queries.List>,
        IHandleQueries<Queries.Catalog>,
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.DescriptionUpdated>,
        IHandleMessages<Events.PictureSet>,
        IHandleMessages<Events.PriceUpdated>,
        IHandleMessages<Events.Removed>,
        IHandleMessages<Events.ReorderMarked>,
        IHandleMessages<Events.ReorderUnMarked>,
        IHandleMessages<Events.StockUpdated>,
        IHandleMessages<Events.ThresholdsUpdated>
    {
        public async Task Handle(Queries.List query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            var results = await ctx.Uow().Query<Models.CatalogProductIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Queries.Catalog query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            if (query.BrandId.HasValue)
                builder.Add("CatalogBrandId", query.BrandId.ToString(), Operation.Equal);
            if (query.TypeId.HasValue)
                builder.Add("CatalogTypeId", query.TypeId.ToString(), Operation.Equal);

            if (!string.IsNullOrEmpty(query.Search))
                builder.Grouped(Group.Any)
                    .Add("Name", query.Search, Operation.Contains)
                    .Add("Description", query.Search, Operation.Contains);

            var results = await ctx.Uow().Query<Models.CatalogProductIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var brand = await ctx.Uow()
                .Get<CatalogBrand.Models.CatalogBrand>(e.CatalogBrandId)
                .ConfigureAwait(false);
            var type = await ctx.Uow()
                .Get<CatalogType.Models.CatalogType>(e.CatalogTypeId)
                .ConfigureAwait(false);

            var model = new Models.CatalogProductIndex
            {
                Id = e.ProductId,
                Name = e.Name,
                Price = e.Price,
                CatalogBrand = brand.Brand,
                CatalogBrandId = brand.Id,
                CatalogType = type.Type,
                CatalogTypeId = type.Id
            };

            await ctx.Uow().Add(e.ProductId, model).ConfigureAwait(false);
        }
        public async Task Handle(Events.DescriptionUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.Description = e.Description;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.PictureSet e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.PictureContents = e.Content;
            product.PictureContentType = e.ContentType;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.PriceUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.Price = e.Price;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.Uow().Delete<Models.CatalogProductIndex>(e.ProductId);
        }

        public async Task Handle(Events.ReorderMarked e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.OnReorder = true;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.ReorderUnMarked e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.OnReorder = false;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.StockUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.AvailableStock = e.Stock;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
        public async Task Handle(Events.ThresholdsUpdated e, IMessageHandlerContext ctx)
        {
            var product = await ctx.Uow().Get<Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            product.MaxStockThreshold = e.MaxStockThreshold;
            product.RestockThreshold = e.RestockThreshold;

            await ctx.Uow().Update(e.ProductId, product).ConfigureAwait(false);
        }
    }
}
