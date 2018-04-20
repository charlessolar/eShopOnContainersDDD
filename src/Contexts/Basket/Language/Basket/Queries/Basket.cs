using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Basket.Basket.Queries
{
    public class Basket : Query
    {
        public Guid BasketId { get; set; }
    }
}
