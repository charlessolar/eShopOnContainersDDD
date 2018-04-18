using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Events
{
    public interface ProductRemoved : IStampedEvent
    {
        Guid ProductId { get; set; }
    }
}
