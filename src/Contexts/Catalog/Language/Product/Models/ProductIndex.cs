using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Catalog.Product.Models
{
    public class ProductIndex
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public long Price { get; set; }

        public Guid CatalogTypeId { get; set; }
        public string CatalogType { get; set; }

        public Guid CatalogBrandId { get; set; }
        public string CatalogBrand { get; set; }

        public decimal AvailableStock { get; set; }
        public decimal RestockThreshold { get; set; }
        public decimal MaxStockThreshold { get; set; }

        public bool OnReorder { get; set; }

        public byte[] PictureContents { get; set; }
        public string PictureContentType { get; set; }
    }
}
