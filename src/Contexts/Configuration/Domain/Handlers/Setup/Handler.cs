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
        public Handler(IContainer container)
        {
            Importer.LoadOperations(container);
        }
        public async Task Handle(Commands.Seed command, IMessageHandlerContext ctx)
        {
            var setup = await ctx.For<Setup>().New("setup").ConfigureAwait(false);

            await Importer.ImportCategory("*").ConfigureAwait(false);

            setup.Seed();
        }
    }
}
