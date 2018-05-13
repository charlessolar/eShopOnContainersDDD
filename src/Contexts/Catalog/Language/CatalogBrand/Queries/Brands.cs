using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Catalog.CatalogBrand.Queries
{
    public class Brands : Paged
    {
        public string Term { get; set; }
        public int Limit { get; set; }

        public Guid? Id { get; set; }
    }
}
