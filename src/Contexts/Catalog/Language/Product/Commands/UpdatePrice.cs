using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Commands
{
    public class UpdatePrice : StampedCommand
    {
        public Guid ProductId { get; set; }
        public long Price { get; set; }
    }
}
