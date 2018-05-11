using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CatalogType.Services
{
    [Api("Catalog")]
    [Route("/catalog/type/{TypeId}", "POST")]
    public class RemoveCatalogType : DomainCommand
    {
        public Guid TypeId { get; set; }
    }
}
