using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CatalogBrand.Services
{
    [Api("Catalog")]
    [Route("/catalog/brand", "POST")]
    public class AddCatalogBrand : DomainCommand
    {
        public Guid BrandId { get; set; }
        public string Brand { get; set; }
    }
}
