using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;
using Infrastructure.ServiceStack;

namespace eShop.Basket.Basket.Services
{
    [Api("Basket")]
    [Route("/basket", "GET")]
    public class GetBasket : Query<Models.Basket>
    {
        public Guid BasketId { get; set; }
    }
}
