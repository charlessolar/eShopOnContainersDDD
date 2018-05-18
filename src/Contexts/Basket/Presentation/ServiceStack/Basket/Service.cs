using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

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

        public Task<object> Any(Services.ListBaskets request)
        {
            return _bus.RequestPaged<Queries.Baskets, Models.BasketIndex>(new Queries.Baskets
            {

            });
        }

        public Task Any(Services.InitiateBasket request)
        {
            var session = GetSession();
            return _bus.CommandToDomain(new Commands.Initiate
            {
                BasketId = request.BasketId,
                UserName = session.IsAuthenticated ? session.UserName : ""
            });
        }

        public Task Any(Services.ClaimBasket request)
        {
            var session = GetSession();

            if (!session.IsAuthenticated)
                throw new HttpError("claiming basket requires authentication");

            return _bus.CommandToDomain(new Commands.ClaimBasket
            {
                BasketId = request.BasketId,
                UserName = session.UserName
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
