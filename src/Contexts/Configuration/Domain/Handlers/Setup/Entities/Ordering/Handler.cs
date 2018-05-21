using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Ordering
{
    public class Handler :
        IHandleMessages<Commands.Seed>
    {
        public async Task Handle(Commands.Seed command, IMessageHandlerContext ctx)
        {
            var setup = await ctx.For<Setup>().Get("setup").ConfigureAwait(false);
            var ordering = await setup.For<Orders>().New("ordering").ConfigureAwait(false);
            await Import.Seed(ctx).ConfigureAwait(false);
            ordering.Seeded();
        }
    }
}
