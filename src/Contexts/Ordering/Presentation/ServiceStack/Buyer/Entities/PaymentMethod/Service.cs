using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.ListPaymentMethods request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.RequestPaged<Queries.PaymentMethods, Models.PaymentMethod>(new Queries.PaymentMethods
            {
                UserName = session.UserName,
                Id = request.Id,
                Term = request.Term
            });
        }
        public Task Any(Services.AddBuyerPaymentMethod request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Add
            {
                UserName = session.UserName,
                PaymentMethodId = request.PaymentMethodId,
                Alias = request.Alias,
                CardholderName = request.CardholderName,
                CardNumber = request.CardNumber,
                Expiration = request.Expiration,
                SecurityNumber = request.SecurityNumber,
                CardType = PaymentMethod.CardType.FromValue(request.CardType)
            });
        }

        public Task Any(Services.RemoveBuyerPaymentMethod request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Remove
            {
                UserName = session.UserName,
                PaymentMethodId = request.PaymentMethodId
            });
        }
    }
}
