using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.CategoryBrand
{
    public class Handler :
        IHandleQueries<Queries.Brands>,
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public async Task Handle(Queries.Brands query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.CategoryBrand>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CategoryBrand
            {
                Id = e.BrandId,
                Brand = e.Brand
            };
            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.BrandId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.CategoryBrand>(e.BrandId);
        }
    }
}
