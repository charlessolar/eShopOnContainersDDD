using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Commands
{
    public interface Removed : IStampedEvent
    {
        Guid LocationId { get; set; }
    }
}
