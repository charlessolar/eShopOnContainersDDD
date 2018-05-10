using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Catalog.Product.Queries
{
    public class Catalog : Paged
    {
        public Guid? BrandId { get; set; }
        public Guid? TypeId { get; set; }
        public string Search { get; set; }
    }
}
