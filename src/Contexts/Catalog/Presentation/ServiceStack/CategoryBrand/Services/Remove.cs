using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CategoryBrand.Services
{
    [Api("Catalog")]
    [Route("/catalog/brand/{BrandId}", "DELETE")]
    public class RemoveCategoryBrand : DomainCommand
    {
        public Guid BrandId { get; set; }
    }
}
