using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Events
{
    public interface AddressChanged : IStampedEvent
    {
        Guid OrderId { get; set; }

        Guid BillingId { get; set; }
        Guid ShippingId { get; set; }
    }
}
