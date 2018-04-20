using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CategoryBrand.Services
{
    [Api("Catalog")]
    [Route("/catalog/brand", "GET")]
    public class ListCategoryBrands : Paged<Models.CategoryBrand>
    {
        public string Term { get; set; }
        public int Limit { get; set; }
    }
}
