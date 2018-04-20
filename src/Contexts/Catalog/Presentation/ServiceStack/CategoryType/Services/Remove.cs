using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.CategoryType.Services
{
    [Api("Catalog")]
    [Route("/catalog/type/{TypeId}", "POST")]
    public class RemoveCategoryType : Command
    {
        public Guid TypeId { get; set; }
    }
}
