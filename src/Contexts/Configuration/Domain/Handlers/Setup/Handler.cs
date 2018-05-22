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
        IHandleMessages<Commands.Seed>
    {
        public async Task Handle(Commands.Seed command, IMessageHandlerContext ctx)
        {
            var setup = await ctx.For<Setup>().New("setup").ConfigureAwait(false);
            setup.Seed();

            await ctx.SendToSelf(new Entities.Identity.Commands.Seed()).ConfigureAwait(false);
            await ctx.SendToSelf(new Entities.Catalog.Commands.Seed()).ConfigureAwait(false);
            await ctx.SendToSelf(new Entities.Basket.Commands.Seed()).ConfigureAwait(false);
            await ctx.SendToSelf(new Entities.Ordering.Commands.Seed()).ConfigureAwait(false);
        }
    }
}
