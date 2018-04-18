using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Events
{
    public interface ProductAdded : IStampedEvent
    {
        Guid ProductId { get; set; }

        string Name { get; set; }
        decimal Price { get; set; }
    }
}
