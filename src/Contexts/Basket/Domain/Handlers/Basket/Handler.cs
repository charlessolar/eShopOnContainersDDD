using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Aggregates.Domain;

namespace eShop.Basket.Basket
{
    public class Handler : 
        IHandleMessages<Commands.Initiate>,
        IHandleMessages<Commands.ClaimBasket>,
        IHandleMessages<Commands.Destroy>
    {
        public async Task Handle(Commands.Initiate command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().New(command.BasketId).ConfigureAwait(false);
            var user = await ctx.For<Identity.User.User>().TryGet(command.UserName).ConfigureAwait(false);
            basket.Initiate(user?.State);
        }
        public async Task Handle(Commands.ClaimBasket command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().Get(command.BasketId).ConfigureAwait(false);
            var user = await ctx.For<Identity.User.User>().Get(command.UserName).ConfigureAwait(false);
            basket.Claim(user.State);
        }
        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var basket = await ctx.For<Basket>().Get(command.BasketId).ConfigureAwait(false);
            basket.Destroy();
        }
    }
}
