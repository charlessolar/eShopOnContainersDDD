using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Payment.Payment
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.BuyerPayments request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.RequestPaged<Queries.BuyerPayments, Models.PaymentIndex>(new Queries.BuyerPayments
            {
                UserName = session.UserName
            });
        }
        public Task<object> Any(Services.PaymentList request)
        {
            return _bus.RequestPaged<Queries.Payments, Models.PaymentIndex>(new Queries.Payments
            {
            });
        }

        public Task Any(Services.Cancel request)
        {
            return _bus.CommandToDomain(new Commands.Cancel
            {
                PaymentId = request.PaymentId
            });
        }
        public Task Any(Services.Settle request)
        {
            return _bus.CommandToDomain(new Commands.Settle
            {
                PaymentId = request.PaymentId
            });
        }
        public Task Any(Services.Charge request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Charge
            {
                PaymentId = request.PaymentId,
                OrderId = request.OrderId,
                UserName = session.UserName
            });
        }
    }
}
