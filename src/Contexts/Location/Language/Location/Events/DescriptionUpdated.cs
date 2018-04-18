using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Commands
{
    public interface DescriptionUpdated : IStampedEvent
    {
        Guid LocationId { get; set; }
        string Description { get; set; }
    }
}
