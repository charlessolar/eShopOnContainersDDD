using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    [Api("Basket")]
    [Route("/basket/item/{ProductId}", "DELETE")]
    public class RemoveBasketItem : DomainCommand
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
    }
}
