using Infrastructure.Commands;
using System;

namespace eShop.Payment.Payment.Events
{
    public interface Canceled : IStampedEvent
    {
        Guid PaymentId { get; set; }
    }
}
