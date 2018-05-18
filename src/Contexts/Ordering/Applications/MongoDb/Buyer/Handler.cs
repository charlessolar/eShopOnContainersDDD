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
        IHandleMessages<Events.Suspended>
    {
        public Task Handle(Events.Initiated e, IMessageHandlerContext ctx)
        {
            var model = new Models.Buyer
            {
                UserName = e.UserName,
                GivenName = e.GivenName,
                GoodStanding = true
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.UserName, model);
        }

        public async Task Handle(Events.InGoodStanding e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Buyer>(e.UserName).ConfigureAwait(false);
            buyer.GoodStanding = true;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.PreferredAddressSet e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Buyer>(e.UserName).ConfigureAwait(false);
            buyer.PreferredAddressId = e.AddressId;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.PreferredPaymentSet e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Buyer>(e.UserName).ConfigureAwait(false);
            buyer.PreferredPaymentMethodId = e.PaymentMethodId;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }
        public async Task Handle(Events.Suspended e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Buyer>(e.UserName).ConfigureAwait(false);
            buyer.GoodStanding = false;
            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.UserName, buyer).ConfigureAwait(false);
        }

    }
}
