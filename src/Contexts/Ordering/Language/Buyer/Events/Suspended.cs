using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Events
{
    public interface Suspended : IStampedEvent
    {
        string UserName { get; set; }
    }
}
