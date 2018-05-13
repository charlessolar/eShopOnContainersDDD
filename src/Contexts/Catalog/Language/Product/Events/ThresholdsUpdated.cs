using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Events
{
    public interface ThresholdsUpdated : IStampedEvent
    {
        Guid ProductId { get; set; }

        decimal RestockThreshold { get; set; }
        decimal MaxStockThreshold { get; set; }
    }
}
