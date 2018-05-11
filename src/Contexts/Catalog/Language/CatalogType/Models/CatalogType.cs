using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CatalogType.Models
{
    public class CatalogType
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}
