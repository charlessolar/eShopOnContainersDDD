using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Basket.Basket.Entities.Item.Events
{
    public interface QuantityUpdated : IStampedEvent
    {
        Guid BasketId { get; set; }
        Guid ItemId { get; set; }

        decimal Quantity { get; set; }
    }
}
