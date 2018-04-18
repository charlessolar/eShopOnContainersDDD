using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Events
{
    public interface Removed : IStampedEvent
    {
        Guid LocationId { get; set; }
    }
}
