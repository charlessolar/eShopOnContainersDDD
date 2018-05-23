using Infrastructure.ServiceStack;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Services
{
    [Api("Ordering")]
    [Route("/orders/sales", "GET")]
    public class SalesChart : Paged<Models.SalesChart>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
