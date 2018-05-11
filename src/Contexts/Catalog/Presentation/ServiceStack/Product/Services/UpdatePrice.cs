using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    [Api("Catalog")]
    [Route("/catalog/products/{ProductId}/price", "POST")]
    public class UpdatePriceProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
        public int Price { get; set; }
    }
}
