using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Events
{
    public interface Added : IStampedEvent
    {
        Guid ProductId { get; set; }

        string Name { get; set; }
        decimal Price { get; set; }

        Guid CategoryBrandId { get; set; }
        Guid CategoryTypeId { get; set; }
    }
}
