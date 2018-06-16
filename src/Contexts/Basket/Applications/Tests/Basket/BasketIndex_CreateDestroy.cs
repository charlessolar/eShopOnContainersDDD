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
    public class BasketIndex_CreateDestroy
    {
        [Theory, AutoFakeItEasyData]
        public async Task ShouldCreateBasketIndex(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.App.Plan<Identity.User.Models.User>("test").Exists(new Identity.User.Models.User { GivenName = "test" });

            var @event = context.Create<Events.Initiated>(x =>
            {
                x.BasketId = context.Id();
                x.UserName = "test";
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Added();
            context.App.Check<Models.BasketIndex>(context.Id()).Added(x => x.Customer == "test");
            context.App.Check<Models.BasketIndex>(context.Id()).Added(new Models.BasketIndex
            {
                Id = context.Id(),
                Customer = "test"
            });
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldDestoyBasketIndex(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.App.Plan<Models.BasketIndex>(context.Id()).Exists();
            var @event = context.Create<Events.Destroyed>(x =>
          {
              x.BasketId = context.Id();
          });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Deleted();
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldCreateBasketIndexWithoutUser(
            TestableContext context,
            BasketIndex handler
            )
        {

            var @event = context.Create<Events.Initiated>(x =>
            {
                x.BasketId = context.Id();
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Added(x => x.Customer == null);
        }
        [Theory, AutoFakeItEasyData]
        public async Task ShouldClaimBucket(
            TestableContext context,
            BasketIndex handler
            )
        {
            context.App.Plan<Identity.User.Models.User>("test").Exists(new Identity.User.Models.User { GivenName = "test" });
            context.App.Plan<Models.BasketIndex>(context.Id()).Exists();

            var @event = context.Create<Events.BasketClaimed>(x =>
            {
                x.BasketId = context.Id();
                x.UserName = "test";
            });

            await handler.Handle(@event, context).ConfigureAwait(false);

            context.App.Check<Models.BasketIndex>(context.Id()).Updated(x => x.Customer == "test");
        }
    }
}
