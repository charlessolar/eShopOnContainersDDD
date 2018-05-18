using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.Address
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.ListAddresses request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.RequestPaged<Queries.Addresses, Models.Address>(new Queries.Addresses
            {
                UserName = session.UserName
            });
        }

        public Task Any(Services.AddBuyerAddress request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Add
            {
                UserName = session.UserName,
                AddressId = request.AddressId,
                Street = request.Street,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                Country = request.Country,
            });
        }

        public Task Any(Services.RemoveBuyerAddress request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Remove
            {
                UserName = session.UserName,
                AddressId = request.AddressId
            });
        }
    }
}
