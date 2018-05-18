using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer
{
    public class Handler : 
        IHandleMessages<Events.Initiated>,
        IHandleMessages<Events.InGoodStanding>,
        IHandleMessages<Events.PreferredAddressSet>,
        IHandleMessages<Events.PreferredPaymentSet>,
        IHandleMessages<Events.Suspended>,
        IHandleMessages<Order.Events.Paid>,
        IHandleMessages<Order.Events.Canceled>
    {
        public Task Handle(Events.Initiated e, IMessageHandlerContext ctx)
        {
            var model = new Models.OrderingBuyerIndex
            {
                Id = e.UserName,
                GivenName = e.GivenName,
                GoodStanding = true
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.UserName, model);
        }

        public async Task Handle(Events.InGoodStanding e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            buyer.GoodStanding = true;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.PreferredAddressSet e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            var address = await ctx.App<Infrastructure.IUnitOfWork>().Get<Entities.Address.Models.Address>(e.AddressId)
                .ConfigureAwait(false);

            buyer.PreferredCity = address.City;
            buyer.PreferredState = address.State;
            buyer.PreferredZipCode = address.ZipCode;
            buyer.PreferredCountry = address.Country;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.PreferredPaymentSet e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            var method = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            buyer.PreferredPaymentCardholder = method.CardholderName;
            buyer.PreferredPaymentMethod = method.CardType;
            buyer.PreferredPaymentExpiration = method.Expiration.ToString("MM/yy");
            
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.Suspended e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            buyer.GoodStanding = false;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Order.Events.Paid e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Order.Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingBuyerIndex>(order.UserName).ConfigureAwait(false);

            buyer.TotalSpent += order.Total;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(order.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Order.Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Order.Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingBuyerIndex>(order.UserName).ConfigureAwait(false);

            if (!order.Paid)
                return;

            buyer.TotalSpent -= order.Total;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(order.UserName, buyer).ConfigureAwait(false);
        }
    }
}
