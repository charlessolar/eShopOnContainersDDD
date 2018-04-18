using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Commands
{
    public interface Added : IStampedEvent
    {
        Guid LocationId { get; set; }
        string Code { get; set; }
        string Description { get; set; }

        Guid ParentId { get; set; }
    }
}
