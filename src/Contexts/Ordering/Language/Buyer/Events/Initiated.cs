using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Events
{
    public interface Initiated : IStampedEvent
    {
        string UserName { get; set; }
        string GivenName { get; set; }
    }
}
