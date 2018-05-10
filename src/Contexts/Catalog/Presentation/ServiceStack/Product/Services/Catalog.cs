using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    [Api("Catalog")]
    [Route("/catalog", "GET")]
    public class Catalog : Paged<Models.ProductIndex>
    {
        public Guid? BrandId { get; set; }
        public Guid? TypeId { get; set; }
        public string Search { get; set; }
    }
}
