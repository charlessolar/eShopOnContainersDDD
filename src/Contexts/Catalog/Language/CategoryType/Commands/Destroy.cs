using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CategoryType.Commands
{
    public class Destroy : StampedCommand
    {
        public Guid TypeId { get; set; }
    }
}
