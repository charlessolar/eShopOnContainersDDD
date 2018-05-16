using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    [Api("Basket")]
    [Route("/basket/item", "GET")]
    public class GetBasketItems : Paged<Models.BasketItemIndex>
    {
        public Guid BasketId { get; set; }
    }
}
