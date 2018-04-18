using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Entities.Point.Events
{
    public interface Removed : IStampedEvent
    {
        Guid LocationId { get; set; }
        Guid PositionId { get; set; }
    }
}
