using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public string Customer { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
