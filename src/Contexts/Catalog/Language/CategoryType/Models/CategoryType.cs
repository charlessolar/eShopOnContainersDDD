using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CategoryType.Models
{
    public class CategoryType
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}
