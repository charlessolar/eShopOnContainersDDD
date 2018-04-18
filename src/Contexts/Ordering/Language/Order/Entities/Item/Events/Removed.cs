using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Entities.Item.Events
{
    public interface Removed : IStampedEvent
    {
        Guid ItemId { get; set; }
        Guid OrderId { get; set; }
    }
}
