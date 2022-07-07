using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Domain;
using NServiceBus;

namespace eShop.Catalog.CatalogType
{
    public class Handler :
        IHandleMessages<Commands.Define>,
        IHandleMessages<Commands.Destroy>
    {
        public async Task Handle(Commands.Define command, IMessageHandlerContext ctx)
        {
            var catType = await ctx.For<Type>().New(command.TypeId).ConfigureAwait(false);
            catType.Define(command.Type);
        }

        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var catType = await ctx.For<Type>().Get(command.TypeId).ConfigureAwait(false);
            catType.Destroy();
        }
    }
}
