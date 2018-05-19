using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Ordering.Buyer
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.Buyers request)
        {
            return _bus.RequestPaged<Queries.Buyers, Models.OrderingBuyerIndex>(new Queries.Buyers
            {
            });
        }
        public Task<object> Any(Services.Buyer request)
        {
            var session = GetSession();

            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.RequestQuery<Queries.Buyer, Models.OrderingBuyer>(new Queries.Buyer
            {
                UserName = session.UserName,
            });
        }

        public Task Any(Services.InitiateBuyer request)
        {
            var session = GetSession();

            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Initiate
            {
                UserName = session.UserName,
                GivenName = session.DisplayName
            });
        }

        public Task Any(Services.MarkGoodStanding request)
        {
            return _bus.CommandToDomain(new Commands.MarkGoodStanding
            {
                UserName = request.UserName
            });
        }
        public Task Any(Services.MarkSuspended request)
        {
            return _bus.CommandToDomain(new Commands.MarkSuspended
            {
                UserName = request.UserName
            });
        }
        public Task Any(Services.SetPreferredAddress request)
        {
            var session = GetSession();

            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.SetPreferredAddress
            {
                UserName = session.UserName,
                AddressId = request.AddressId
                
            });
        }
        public Task Any(Services.SetPreferredPaymentMethod request)
        {
            var session = GetSession();

            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.SetPreferredPaymentMethod
            {
                UserName = session.UserName,
                PaymentMethodId = request.PaymentMethodId
            });
        }
    }
}
