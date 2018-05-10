using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    [Api("Catalog")]
    [Route("/catalog/products", "POST")]
    public class AddProduct : DomainCommand
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public Guid CategoryBrandId { get; set; }
        public Guid CategoryTypeId { get; set; }
    }
}
