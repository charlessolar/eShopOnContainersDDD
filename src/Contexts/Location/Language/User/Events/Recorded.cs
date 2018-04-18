using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.User.Events
{
    public interface Recorded : IStampedEvent
    {
        Guid RecordId { get; set; }

        Guid LocationId { get; set; }
        Guid UserId { get; set; }
    }
}
