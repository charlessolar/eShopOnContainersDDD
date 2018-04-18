using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Events
{
    public interface Paid : IStampedEvent
    {
        Guid OrderId { get; set; }
    }
}
