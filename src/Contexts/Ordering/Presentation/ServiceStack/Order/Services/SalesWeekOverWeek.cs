using Infrastructure.ServiceStack;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Services
{
    [Api("Ordering")]
    [Route("/orders/sales_week_over_week", "GET")]
    public class SalesWeekOverWeek : Paged<Models.SalesWeekOverWeek>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
