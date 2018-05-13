using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CatalogBrand.Services
{
    [Api("Catalog")]
    [Route("/catalog/brand", "GET")]
    public class ListCatalogBrands : Paged<Models.CatalogBrand>
    {
        public string Term { get; set; }
        public int Limit { get; set; }

        public Guid? Id { get; set; }
    }
}
