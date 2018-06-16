using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Ordering.Buyer
{
    public class Handler : 
        IHandleQueries<Queries.Buyers>,
        IHandleMessages<Events.Initiated>,
        IHandleMessages<Events.InGoodStanding>,
        IHandleMessages<Events.PreferredAddressSet>,
        IHandleMessages<Events.PreferredPaymentSet>,
        IHandleMessages<Events.Suspended>,
        IHandleMessages<Order.Events.Drafted>,
        IHandleMessages<Order.Events.Paid>,
        IHandleMessages<Order.Events.Canceled>
    {
        public async Task Handle(Queries.Buyers query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            var results = await ctx.UoW().Query<Models.OrderingBuyerIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public Task Handle(Events.Initiated e, IMessageHandlerContext ctx)
        {
            var model = new Models.OrderingBuyerIndex
            {
                Id = e.UserName,
                GivenName = e.GivenName,
                GoodStanding = true
            };

            return ctx.UoW().Add(e.UserName, model);
        }

        public async Task Handle(Events.InGoodStanding e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            buyer.GoodStanding = true;
            await ctx.UoW().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.PreferredAddressSet e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            var address = await ctx.UoW().Get<Entities.Address.Models.Address>(e.AddressId)
                .ConfigureAwait(false);

            buyer.PreferredCity = address.City;
            buyer.PreferredState = address.State;
            buyer.PreferredZipCode = address.ZipCode;
            buyer.PreferredCountry = address.Country;

            await ctx.UoW().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.PreferredPaymentSet e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            var method = await ctx.UoW()
                .Get<Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            buyer.PreferredPaymentCardholder = method.CardholderName;
            buyer.PreferredPaymentMethod = method.CardType;
            buyer.PreferredPaymentExpiration = method.Expiration.ToString("MM/yy");
            
            await ctx.UoW().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.Suspended e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            buyer.GoodStanding = false;
            await ctx.UoW().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Order.Events.Drafted e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            buyer.TotalOrders++;
            buyer.LastOrder = e.Stamp;
            await ctx.UoW().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Order.Events.Paid e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Order.Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(order.UserName).ConfigureAwait(false);

            buyer.TotalSpent += order.Total;
            buyer.TotalOrders++;

            await ctx.UoW().Update(order.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Order.Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Order.Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var buyer = await ctx.UoW().Get<Models.OrderingBuyerIndex>(order.UserName).ConfigureAwait(false);

            if (order.Paid)
            {
                buyer.TotalSpent -= order.Total;
            }

            buyer.TotalOrders--;

            await ctx.UoW().Update(order.UserName, buyer).ConfigureAwait(false);
        }
    }
}
