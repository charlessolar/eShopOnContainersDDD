using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Commands
{
    public class Add : StampedCommand
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
