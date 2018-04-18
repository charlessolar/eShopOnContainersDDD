using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Entities.Item.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set;}

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public decimal Quantity { get; set; }
    }
}
