using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Commands
{
    public class UpdateThresholds : StampedCommand
    {
        public Guid ProductId { get; set; }

        public decimal RestockThreshold { get; set; }
        public decimal MaxStockThreshold { get; set; }
    }
}
