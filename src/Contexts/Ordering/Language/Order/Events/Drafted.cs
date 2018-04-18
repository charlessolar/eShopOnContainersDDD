using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Events
{
    public interface Drafted : IStampedEvent
    {
        Guid OrderId { get; set; }

        Guid BuyerId { get; set; }
        Guid CartId { get; set; }
    }
}
