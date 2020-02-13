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
            var saga = ctx.Saga(Guid.NewGuid());

            saga.Command(new Entities.Identity.Commands.Seed())
                .Command(new Entities.Catalog.Commands.Seed())
                .Command(new Entities.Basket.Commands.Seed())
                .Command(new Entities.Ordering.Commands.Seed());
            await saga.Start().ConfigureAwait(false);
        }
    }
}
