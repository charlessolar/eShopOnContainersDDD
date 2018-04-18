using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Catalog.CategoryBrand
{
    public class Handler :
        IHandleMessages<Commands.Define>,
        IHandleMessages<Commands.Destroy>
    {
        public async Task Handle(Commands.Define command, IMessageHandlerContext ctx)
        {
            var brand = await ctx.For<Brand>().New(command.BrandId).ConfigureAwait(false);
            brand.Define(command.Brand);
        }

        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var brand = await ctx.For<Brand>().Get(command.BrandId).ConfigureAwait(false);
            brand.Destroy();
        }
    }
}
