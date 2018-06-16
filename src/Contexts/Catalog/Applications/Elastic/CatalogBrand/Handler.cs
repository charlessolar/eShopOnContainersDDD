using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.CatalogBrand
{
    public class Handler :
        IHandleQueries<Queries.Brands>,
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public async Task Handle(Queries.Brands query, IMessageHandlerContext ctx)
        {
            if (query.Id.HasValue)
            {
                var type = await ctx.UoW().Get<Models.CatalogBrand>(query.Id.Value)
                    .ConfigureAwait(false);

                await ctx.Result(new[] { type }, 1, 0).ConfigureAwait(false);
                return;
            }

            var builder = new QueryBuilder();
            var results = await ctx.UoW().Query<Models.CatalogBrand>(builder.Build())
                .ConfigureAwait(false);
            if (!string.IsNullOrEmpty(query.Term))
                builder.Add("Brand", query.Term, Operation.Contains);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CatalogBrand
            {
                Id = e.BrandId,
                Brand = e.Brand
            };
            return ctx.UoW().Add(e.BrandId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.CatalogBrand>(e.BrandId);
        }
    }
}
