using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Events
{
    public interface Removed : IStampedEvent
    {
        string UserName { get; set; }
        Guid PaymentMethodId { get; set; }
    }
}
