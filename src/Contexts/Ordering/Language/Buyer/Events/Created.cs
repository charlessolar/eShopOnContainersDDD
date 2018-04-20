using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Events
{
    public interface Created : IStampedEvent
    {
        Guid BuyerId { get; set; }
        string GivenName { get; set; }
    }
}
