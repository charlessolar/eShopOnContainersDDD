using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CatalogBrand.Services
{
    [Api("Catalog")]
    [Route("/catalog/brand/{BrandId}", "DELETE")]
    public class RemoveCatalogBrand : DomainCommand
    {
        public Guid BrandId { get; set; }
    }
}
