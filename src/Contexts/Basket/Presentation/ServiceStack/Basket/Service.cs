using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Basket.Basket
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.GetBasket request)
        {
            return _bus.RequestQuery<Queries.Basket, Models.Basket>(new Queries.Basket
            {
                BasketId = request.BasketId
            });
        }

        public Task Any(Services.BasketDestroy request)
        {
            return _bus.CommandToDomain(new Commands.Destroy
            {
                BasketId = request.BasketId
            });
        }
    }
}
