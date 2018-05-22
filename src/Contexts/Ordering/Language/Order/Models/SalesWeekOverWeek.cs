using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Models
{
    public class SalesWeekOverWeek
    {
        public string Id { get; set; }
        public long Relevancy { get; set; }

        public string DayOfWeek { get; set; }
        public long Value { get; set; }
    }
}
