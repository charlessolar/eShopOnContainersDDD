using Infrastructure.Commands;
using System;

namespace eShop.Payment.Payment.Events
{
    public interface Charged : IStampedEvent
    {
        Guid PaymentId { get; set; }

        string UserName { get; set; }

        Guid OrderId { get; set; }
        Guid PaymentMethodId { get; set; }
    }
}
