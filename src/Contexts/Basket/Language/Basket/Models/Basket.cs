using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public string CustomerId { get; set; }
        public string Customer { get; set; }

        public int TotalItems { get; set; }
        public long TotalQuantity { get; set; }

        public long SubTotal { get; set; }

        public long Created { get; set; }
        public long Updated { get; set; }
    }
}
