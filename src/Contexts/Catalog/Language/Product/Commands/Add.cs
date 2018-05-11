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
        public int Price { get; set; }

        public Guid CatalogBrandId { get; set; }
        public Guid CatalogTypeId { get; set; }
    }
}
