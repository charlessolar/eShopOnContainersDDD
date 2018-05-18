using System;
using System.Collections.Generic;
using System.Text;
using Aggregates;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    public class ItemsInBasket : IService<Guid[]>
    {
        public Guid BasketId { get; set; }
    }
}
