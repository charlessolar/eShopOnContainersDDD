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
            return _bus.RequestPaged<Queries.Addresses, Models.Address>(new Queries.Addresses
            {
                BuyerId = request.BuyerId
            });
        }

        public Task Any(Services.AddBuyerAddress request)
        {
            return _bus.CommandToDomain(new Commands.Add
            {
                BuyerId = request.BuyerId,
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
            return _bus.CommandToDomain(new Commands.Remove
            {
                BuyerId = request.BuyerId,
                AddressId = request.AddressId
            });
        }
    }
}
