using Infrastructure.Commands;
using System;

namespace eShop.Payment.Payment.Events
{
    public interface Settled : IStampedEvent
    {
        Guid PaymentId { get; set; }
    }
}
