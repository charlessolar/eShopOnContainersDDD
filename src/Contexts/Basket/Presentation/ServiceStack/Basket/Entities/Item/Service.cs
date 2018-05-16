using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Basket.Basket.Entities.Item
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.GetBasketItems request)
        {
            return _bus.RequestPaged<Queries.Items, Models.Item>(new Queries.Items
            {
                BasketId = request.BasketId
            });
        }

        public Task Any(Services.AddBasketItem request)
        {
            return _bus.CommandToDomain(new Commands.AddItem
            {
                BasketId = request.BasketId,
                ProductId = request.ProductId,
            });
        }
        public Task Any(Services.RemoveBasketItem request)
        {
            return _bus.CommandToDomain(new Commands.RemoveItem
            {
                BasketId = request.BasketId,
                ProductId = request.ProductId,
            });
        }
        public Task Any(Services.UpdateBasketItemQuantity request)
        {
            return _bus.CommandToDomain(new Commands.UpdateQuantity
            {
                BasketId = request.BasketId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }
    }
}
