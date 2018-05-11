using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Commands
{
    public class UpdateStock : StampedCommand
    {
        public Guid ProductId { get; set; }
        public decimal Stock { get; set; }
    }
}
