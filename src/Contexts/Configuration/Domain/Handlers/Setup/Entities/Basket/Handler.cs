using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Basket
{
    public class Handler :
        IHandleMessages<Commands.Seed>
    {
        public async Task Handle(Commands.Seed command, IMessageHandlerContext ctx)
        {
            var setup = await ctx.For<Setup>().Get("setup").ConfigureAwait(false);
            var basket = await setup.For<Basket>().New("basket").ConfigureAwait(false);
            await Import.Seed(ctx).ConfigureAwait(false);
            basket.Seeded();
        }
    }

}
