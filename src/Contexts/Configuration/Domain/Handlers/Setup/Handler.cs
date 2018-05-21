using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
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

            await ctx.Send(new Entities.Identity.Commands.Seed()).ConfigureAwait(false);
            await ctx.Send(new Entities.Catalog.Commands.Seed()).ConfigureAwait(false);
            await ctx.Send(new Entities.Ordering.Commands.Seed()).ConfigureAwait(false);
            
        }
    }
}
