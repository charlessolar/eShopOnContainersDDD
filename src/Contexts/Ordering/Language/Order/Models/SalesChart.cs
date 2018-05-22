using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Models
{
    public class SalesChart
    {
        public string Id { get; set; }
        public long Relevancy { get; set; }

        public string Label { get; set; }
        public long Value { get; set; }
    }
}
