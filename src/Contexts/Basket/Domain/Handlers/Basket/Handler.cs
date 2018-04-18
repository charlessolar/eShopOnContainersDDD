using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Aggregates;

namespace eShop.Basket.Basket
{
    public class Handler : 
        IHandleMessages<Commands.Destroy>
    {
        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().Get(command.BasketId).ConfigureAwait(false);
            basket.Destroy();
        }
    }
}
