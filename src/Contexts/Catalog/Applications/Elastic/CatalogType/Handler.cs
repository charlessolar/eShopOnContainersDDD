using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.CatalogType
{
    public class Handler : 
        IHandleQueries<Queries.Types>,
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public async Task Handle(Queries.Types query, IMessageHandlerContext ctx)
        {
            if (query.Id.HasValue)
            {
                var type = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.CatalogType>(query.Id.Value)
                    .ConfigureAwait(false);

                await ctx.Result(new[] {type}, 1, 0).ConfigureAwait(false);
                return;
            }

            var builder = new QueryBuilder();
            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.CatalogType>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CatalogType
            {
                Id = e.TypeId,
                Type = e.Type
            };
            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.TypeId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.CatalogType>(e.TypeId);
        }
    }
}
