using Aggregates.Domain;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Identity
{
    public class Handler : 
        IHandleMessages<Commands.Seed>
    {
        public async Task Handle(Commands.Seed command, IMessageHandlerContext ctx)
        {
            var setup = await ctx.For<Setup>().Get("setup").ConfigureAwait(false);
            var users = await setup.For<Identity>().New("identity").ConfigureAwait(false);
            await Import.Seed(ctx).ConfigureAwait(false);
            users.Seeded();
        }
    }
}
