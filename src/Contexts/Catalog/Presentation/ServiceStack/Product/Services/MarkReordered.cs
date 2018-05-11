using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    [Api("Catalog")]
    [Route("/catalog/products/{ProductId}/mark", "POST")]
    public class MarkReordered : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
