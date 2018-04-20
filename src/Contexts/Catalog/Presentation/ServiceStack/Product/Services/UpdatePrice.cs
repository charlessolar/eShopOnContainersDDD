using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    public class UpdatePriceProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
    }
}
