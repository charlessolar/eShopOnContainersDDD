using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    [Api("Basket")]
    [Route("/basket/item/{ItemId}", "DELETE")]
    public class RemoveBasketItem : Command
    {
        public Guid BasketId { get; set; }
        public Guid ItemId { get; set; }
    }
}
