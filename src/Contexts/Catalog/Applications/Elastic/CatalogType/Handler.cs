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
                var type = await ctx.UoW().Get<Models.CatalogType>(query.Id.Value)
                    .ConfigureAwait(false);

                await ctx.Result(new[] {type}, 1, 0).ConfigureAwait(false);
                return;
            }

            var builder = new QueryBuilder();
            var results = await ctx.UoW().Query<Models.CatalogType>(builder.Build())
                .ConfigureAwait(false);
            if (!string.IsNullOrEmpty(query.Term))
                builder.Add("Type", query.Term, Operation.Contains);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CatalogType
            {
                Id = e.TypeId,
                Type = e.Type
            };
            return ctx.UoW().Add(e.TypeId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.CatalogType>(e.TypeId);
        }
    }
}
