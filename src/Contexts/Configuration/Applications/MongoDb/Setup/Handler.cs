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
        IHandleMessages<Events.Seeded>,
        IHandleMessages<Entities.Catalog.Events.Seeded>,
        IHandleMessages<Entities.Ordering.Events.Seeded>,
        IHandleMessages<Entities.Identity.Events.Seeded>,
        IHandleMessages<Entities.Basket.Events.Seeded>
    {
        public async Task Handle(Queries.Status query, IMessageHandlerContext ctx)
        {
            var model = await ctx.Uow().Get<Models.ConfigurationStatus>("setup").ConfigureAwait(false);
            await ctx.Result(model ?? new Models.ConfigurationStatus { IsSetup = false }).ConfigureAwait(false);
        }
        public Task Handle(Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = new Models.ConfigurationStatus
            {
                Id = "setup",
                IsSetup = true
            };

            return ctx.UoW().Add("setup", model);
        }
        public async Task Handle(Entities.Catalog.Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = await ctx.UoW().Get<Models.ConfigurationStatus>("setup").ConfigureAwait(false);
            model.SetupContexts = model.SetupContexts.TryAdd("catalog");

            await ctx.UoW().Update("setup", model).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Ordering.Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = await ctx.UoW().Get<Models.ConfigurationStatus>("setup").ConfigureAwait(false);
            model.SetupContexts = model.SetupContexts.TryAdd("ordering");

            await ctx.UoW().Update("setup", model).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Identity.Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = await ctx.UoW().Get<Models.ConfigurationStatus>("setup").ConfigureAwait(false);
            model.SetupContexts = model.SetupContexts.TryAdd("identity");

            await ctx.UoW().Update("setup", model).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Basket.Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = await ctx.UoW().Get<Models.ConfigurationStatus>("setup").ConfigureAwait(false);
            model.SetupContexts = model.SetupContexts.TryAdd("basket");

            await ctx.UoW().Update("setup", model).ConfigureAwait(false);
        }
    }
}
