using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure.Extensions;
using NServiceBus;
using StructureMap;


namespace eShop.Configuration.Setup
{
    public class Handler :
        IHandleMessages<Commands.Seed>,
        IHandleMessages<Events.Seeded>
    {
        public async Task Handle(Commands.Seed command, IMessageHandlerContext ctx)
        {
            var setup = await ctx.For<Setup>().New("setup").ConfigureAwait(false);
            setup.Seed();
        }
        public async Task Handle(Events.Seeded e, IMessageHandlerContext ctx)
        {
            await ctx.LocalSaga(async bus =>
            {
                await bus.CommandToDomain(new Entities.Identity.Commands.Seed()).ConfigureAwait(false);
                await bus.CommandToDomain(new Entities.Catalog.Commands.Seed()).ConfigureAwait(false);
                await bus.CommandToDomain(new Entities.Basket.Commands.Seed()).ConfigureAwait(false);
                await bus.CommandToDomain(new Entities.Ordering.Commands.Seed()).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
