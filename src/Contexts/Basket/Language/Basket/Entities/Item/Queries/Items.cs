using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Basket.Basket.Entities.Item.Queries
{
    public class Items : Paged
    {
        public Guid BasketId { get; set; }
    }
}
