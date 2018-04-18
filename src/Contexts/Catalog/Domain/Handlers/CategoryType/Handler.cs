using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using eShop.Catalog.CategoryBrand;
using NServiceBus;

namespace eShop.Catalog.CategoryType
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
