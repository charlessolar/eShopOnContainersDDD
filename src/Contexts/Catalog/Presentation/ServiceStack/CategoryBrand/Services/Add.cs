using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CategoryBrand.Services
{
    [Api("Catalog")]
    [Route("/catalog/brand", "POST")]
    public class AddCategoryBrand : Command
    {
        public Guid BrandId { get; set; }
        public string Brand { get; set; }
    }
}
