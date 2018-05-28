using Aggregates;
using Aggregates.Exceptions;
using Infrastructure.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eShop.Basket.Basket
{
    public class claim
    {
        [Theory, AutoFakeItEasyData]
        public async Task Should_claim_basket(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Identity.User.User>(context.Id()).Exists();
            context.UoW.Plan<Basket>(context.Id()).HasEvent<Events.Initiated>(x => { x.BasketId = context.Id(); });

            await handler.Handle(new Commands.ClaimBasket
            {
                BasketId = context.Id(),
                UserName = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Basket>(context.Id()).Raised<Events.BasketClaimed>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_not_claim_basket(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Identity.User.User>(context.Id()).Exists();
            context.UoW.Plan<Basket>(context.Id())
                .HasEvent<Events.Initiated>(x => { x.BasketId = context.Id(); })
                .HasEvent<Events.BasketClaimed>(x => { x.BasketId = context.Id(); x.UserName = context.Id(); });

            await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(new Commands.ClaimBasket
            {
                BasketId = context.Id(),
                UserName = context.Id()
            }, context)).ConfigureAwait(false);
        }
    }
}
