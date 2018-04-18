using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CategoryBrand.Commands
{
    public class Define : StampedCommand
    {
        public Guid BrandId { get; set; }
        public string Brand { get; set; }
    }
}
