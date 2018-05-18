using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Events
{
    public interface Added : IStampedEvent
    {
        Guid PaymentMethodId { get; set; }
        string UserName { get; set; }

        string Alias { get; set; }
        string CardNumber { get; set; }
        string SecurityNumber { get; set; }
        string CardholderName { get; set; }

        DateTime Expiration { get; set; }

        CardType CardType { get; set; }
    }
}
