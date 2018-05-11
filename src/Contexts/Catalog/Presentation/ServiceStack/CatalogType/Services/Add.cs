using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CatalogType.Services
{
    [Api("Catalog")]
    [Route("/catalog/type", "POST")]
    public class AddCatalogType : DomainCommand
    {
        public Guid TypeId { get; set; }
        public string Type { get; set; }
    }
}
