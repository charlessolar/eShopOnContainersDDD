using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Entities.Item.Events
{
    public interface Added : IStampedEvent
    {
        Guid ProductId { get; set; }
        Guid OrderId { get; set; }

        long Quantity { get; set; }
    }
}
