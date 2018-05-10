using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Configuration.Setup
{
    public class Handler :
        IHandleQueries<Queries.Status>,
        IHandleMessages<Events.Seeded>
    {
        public async Task Handle(Queries.Status query, IMessageHandlerContext ctx)
        {
            var model = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Status>("setup").ConfigureAwait(false);
            await ctx.Result(model ?? new Models.Status { IsSetup = false }).ConfigureAwait(false);
        }
        public Task Handle(Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = new Models.Status
            {
                Id = "setup",
                IsSetup = true
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add("setup", model);
        }
    }
}
