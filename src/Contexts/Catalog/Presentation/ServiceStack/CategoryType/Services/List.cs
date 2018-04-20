using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CategoryType.Services
{
    [Api("Catalog")]
    [Route("/catalog/type", "GET")]
    public class ListCategoryTypes : Paged<Models.CategoryType>
    {
        public string Term { get; set; }
        public int Limit { get; set; }
    }
}
