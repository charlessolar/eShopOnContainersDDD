using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{

    [Api("Catalog")]
    [Route("/catalog/products", "GET")]
    public class ListProducts : Paged<Models.CatalogProductIndex>
    {
    }
}
