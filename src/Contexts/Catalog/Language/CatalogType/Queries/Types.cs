using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Catalog.CatalogType.Queries
{
    public class Types : Paged
    {
        public string Term { get; set; }
        public int Limit { get; set; }
    }
}
