using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Order.Queries
{
    public class Order : Query
    {
        public Guid OrderId { get; set; }
    }
}
