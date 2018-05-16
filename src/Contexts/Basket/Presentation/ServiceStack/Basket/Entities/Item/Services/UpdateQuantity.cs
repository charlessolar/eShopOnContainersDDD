using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    [Api("Basket")]
    [Route("/basket/item/{ProductId}/quantity", "POST")]
    public class UpdateBasketItemQuantity : DomainCommand
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }

        public long Quantity { get; set; }
    }
}
