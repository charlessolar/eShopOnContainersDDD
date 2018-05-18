using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Events
{
    public interface PreferredPaymentSet : IStampedEvent
    {
        string UserName { get; set; }
        Guid PaymentMethodId { get; set; }
    }
}
