using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Events
{
    public interface PreferredAddressSet : IStampedEvent
    {
        string UserName { get; set; }
        Guid AddressId { get; set; }
    }
}
