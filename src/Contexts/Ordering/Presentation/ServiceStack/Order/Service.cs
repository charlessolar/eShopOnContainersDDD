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
            return _bus.RequestQuery<Queries.Order, Models.OrderingOrder>(new Queries.Order
            {
                OrderId = request.OrderId
            });
        }

        public Task<object> Any(Services.ListOrders request)
        {
            return _bus.RequestPaged<Queries.Orders, Models.OrderingOrderIndex>(new Queries.Orders
            {
                OrderStatus = string.IsNullOrEmpty(request.OrderStatus) ? null : Status.FromValue(request.OrderStatus),
                From = request.From,
                To = request.To
            });
        }

        public Task<object> Any(Services.BuyerOrders request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.RequestPaged<Queries.BuyerOrders, Models.OrderingOrder>(new Queries.BuyerOrders
            {
                UserName = session.UserName,
                OrderStatus = string.IsNullOrEmpty(request.OrderStatus) ? null : Status.FromValue(request.OrderStatus),
                From = request.From,
                To = request.To
            });
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
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");

            return _bus.CommandToDomain(new Commands.Draft
            {
                OrderId = request.OrderId,
                UserName = session.UserName,
                BasketId = request.BasketId,
                ShippingAddressId = request.ShippingAddressId,
                BillingAddressId = request.BillingAddressId,
                PaymentMethodId = request.PaymentMethodId
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

        public Task Any(Services.ChangeAddressOrder request)
        {
            return _bus.CommandToDomain(new Commands.ChangeAddress
            {
                OrderId = request.OrderId,
                ShippingId = request.ShippingId,
                BillingId = request.BillingId
            });
        }

        public Task Any(Services.ChangePaymentMethodOrder request)
        {
            return _bus.CommandToDomain(new Commands.ChangePaymentMethod
            {
                OrderId = request.OrderId,
                PaymentMethodId = request.PaymentMethodId
            });
        }
    }
}
