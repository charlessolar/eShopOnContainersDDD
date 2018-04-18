using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Events
{
    public interface AddressSet : IStampedEvent
    {
        Guid OrderId { get; set; }

        Guid AddressId { get; set; }
    }
}
