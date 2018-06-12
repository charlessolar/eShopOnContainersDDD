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
    public class create_destroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task Should_create_basket(
            TestableContext context,
            Handler handler
            )
        {
            await handler.Handle(new Commands.Initiate
            {
                BasketId = context.Id(),
                UserName = "test"
            }, context).ConfigureAwait(false);

            context.UoW.Check<Basket>(context.Id()).Raised<Events.Initiated>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_destroy_basket(
            TestableContext context,
            Handler handler
            )
        {
            context.UoW.Plan<Basket>(context.Id()).HasEvent<Events.Initiated>(x =>
            {
                x.BasketId = context.Id();
            });

            await handler.Handle(new Commands.Destroy
            {
                BasketId = context.Id()
            }, context).ConfigureAwait(false);

            context.UoW.Check<Basket>(context.Id()).Raised<Events.Destroyed>();
        }
        [Theory, AutoFakeItEasyData]
        public async Task Should_not_destroy_unknown(
            TestableContext context,
            Handler handler
            )
        {
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new Commands.Destroy
            {
                BasketId = context.Id()
            }, context)).ConfigureAwait(false);

        }
    }
}
