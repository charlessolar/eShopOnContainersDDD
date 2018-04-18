using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Events
{
    public interface Defined : IStampedEvent
    {
        Guid Id { get; set; }

        string Name { get; set; }
        string Description { get; set; }
    }
}
