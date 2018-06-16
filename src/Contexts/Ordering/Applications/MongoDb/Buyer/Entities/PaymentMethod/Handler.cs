using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod
{
    public class Handler :
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.Removed>
    {
        public Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var model = new Models.PaymentMethod
            {
                Id = e.PaymentMethodId,
                UserName = e.UserName,
                Alias =e.Alias,
                CardholderName=e.CardholderName,
                CardNumber=e.CardNumber,
                Expiration=e.Expiration,
                SecurityNumber = e.SecurityNumber,
                CardType = e.CardType.Value
            };

            return ctx.UoW().Add(e.PaymentMethodId, model);
        }

        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.PaymentMethod>(e.PaymentMethodId);
        }
    }
}
