using Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Queries
{
    public class SalesByState : Paged
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
