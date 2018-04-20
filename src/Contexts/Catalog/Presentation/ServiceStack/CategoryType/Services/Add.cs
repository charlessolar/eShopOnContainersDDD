using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CategoryType.Services
{
    [Api("Catalog")]
    [Route("/catalog/type", "POST")]
    public class AddCategoryType : DomainCommand
    {
        public Guid TypeId { get; set; }
        public string Type { get; set; }
    }
}
