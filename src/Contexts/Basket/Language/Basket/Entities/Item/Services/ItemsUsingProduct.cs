using System;
using System.Collections.Generic;
using System.Text;
using Aggregates;

namespace eShop.Basket.Basket.Entities.Item.Services
{
    public class ItemsUsingProduct : IService<Guid[]>
    {
        public Guid ProductId { get; set; }
    }
}
