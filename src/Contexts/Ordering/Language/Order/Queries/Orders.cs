using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Order.Queries
{
    public class Orders : Paged
    {
        public Status OrderStatus { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
