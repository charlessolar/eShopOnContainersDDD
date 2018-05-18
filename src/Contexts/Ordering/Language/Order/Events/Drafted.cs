using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Events
{
    public interface Drafted : IStampedEvent
    {
        Guid OrderId { get; set; }

        string UserName { get; set; }
        Guid BasketId { get; set; }

        Guid AddressId { get; set; }
        Guid PaymentMethodId { get; set; }
    }
}
