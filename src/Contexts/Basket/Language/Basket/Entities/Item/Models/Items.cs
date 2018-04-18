using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket.Entities.Item.Models
{
    public class Items
    {
        public Guid BasketId { get; set; }
        public Guid ItemId { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        
        public decimal Quantity { get; set; }

        public decimal Total { get; set; }
    }
}
