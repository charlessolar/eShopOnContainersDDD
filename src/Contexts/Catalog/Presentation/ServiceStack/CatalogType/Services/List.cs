using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CatalogType.Services
{
    [Api("Catalog")]
    [Route("/catalog/type", "GET")]
    public class ListCatalogTypes : Paged<Models.CatalogType>
    {
        public string Term { get; set; }
        public int Limit { get; set; }

        public Guid? Id { get; set; }
    }
}
