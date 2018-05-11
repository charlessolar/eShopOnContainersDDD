using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Catalog.Product
{
    public class Handler : 
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.Remove>,
        IHandleMessages<Commands.SetPicture>,
        IHandleMessages<Commands.UpdateDescription>,
        IHandleMessages<Commands.UpdatePrice>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var product = await ctx.For<Product>().New(command.ProductId).ConfigureAwait(false);
            var brand = await ctx.For<CatalogBrand.Brand>().Get(command.CatalogBrandId).ConfigureAwait(false);
            var type = await ctx.For<CatalogType.Type>().Get(command.CatalogTypeId).ConfigureAwait(false);

            product.Add(command.Name, command.Price, brand.State, type.State);
        }
        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var product = await ctx.For<Product>().Get(command.ProductId).ConfigureAwait(false);
            product.Remove();
        }
        public async Task Handle(Commands.SetPicture command, IMessageHandlerContext ctx)
        {
            var product = await ctx.For<Product>().Get(command.ProductId).ConfigureAwait(false);
            product.SetPicture(command.Content, command.ContentType);
        }
        public async Task Handle(Commands.UpdateDescription command, IMessageHandlerContext ctx)
        {
            var product = await ctx.For<Product>().Get(command.ProductId).ConfigureAwait(false);
            product.UpdateDescription(command.Description);
        }
        public async Task Handle(Commands.UpdatePrice command, IMessageHandlerContext ctx)
        {
            var product = await ctx.For<Product>().Get(command.ProductId).ConfigureAwait(false);
            product.UpdatePrice(command.Price);
        }

    }
}
