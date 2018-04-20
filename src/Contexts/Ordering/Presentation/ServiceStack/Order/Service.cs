using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Ordering.Order
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.GetOrder request)
        {
            return _bus.RequestQuery<Queries.Order, Models.Order>(new Queries.Order
            {
                OrderId=request.OrderId
            });
        }

        public Task<object> Any(Services.ListOrders request)
        {
            return _bus.RequestPaged<Queries.Orders, Models.OrderIndex>(new Queries.Orders { });
        }

        public Task Any(Services.CancelOrder request)
        {
            return _bus.CommandToDomain(new Commands.Cancel
            {
                OrderId = request.OrderId
            });
        }
        public Task Any(Services.ConfirmOrder request)
        {
            return _bus.CommandToDomain(new Commands.Confirm
            {
                OrderId = request.OrderId
            });
        }
        public Task Any(Services.DraftOrder request)
        {
            return _bus.CommandToDomain(new Commands.Draft
            {
                OrderId = request.OrderId,
                BuyerId = request.BuyerId,
                CartId=request.BasketId
            });
        }
        public Task Any(Services.PayOrder request)
        {
            return _bus.CommandToDomain(new Commands.Pay
            {
                OrderId = request.OrderId
            });
        }
        public Task Any(Services.ShipOrder request)
        {
            return _bus.CommandToDomain(new Commands.Ship
            {
                OrderId = request.OrderId
            });
        }

        public Task Any(Services.SetAddressOrder request)
        {
            return _bus.CommandToDomain(new Commands.SetAddress
            {
                OrderId = request.OrderId,
                AddressId = request.AddressId
            });
        }

        public Task Any(Services.SetPaymentMethodOrder request)
        {
            return _bus.CommandToDomain(new Commands.SetPaymentMethod
            {
                OrderId = request.OrderId,
                PaymentMethodId = request.PaymentMethodId
            });
        }
    }
}
