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
            return _bus.RequestPaged<Queries.Buyers, Models.Buyer>(new Queries.Buyers
            {
            });
        }

        public Task Any(Services.CreateBuyer request)
        {
            return _bus.CommandToDomain(new Commands.Create
            {
                BuyerId = request.BuyerId,
                GivenName = request.GivenName
            });
        }
    }
}
