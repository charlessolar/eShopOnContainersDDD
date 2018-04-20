using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Catalog.Product.Queries
{
    public class Product : Query
    {
        public Guid ProductId { get; set; }
    }
}
