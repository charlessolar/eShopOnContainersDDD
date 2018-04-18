using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Commands
{
    public class UpdateDescription : StampedCommand
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; }
    }
}
