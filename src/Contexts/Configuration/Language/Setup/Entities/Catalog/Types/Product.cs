using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Catalog.Types
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }

        public Guid CatalogTypeId { get; set; }
        public Guid CatalogBrandId { get; set; }

        public decimal AvailableStock { get; set; }
        public bool OnReorder { get; set; }

        public decimal RestockThreshold { get; set; }
        public decimal MaxStockThreshold { get; set; }

        public string Picture { get; set; }
    }
}
