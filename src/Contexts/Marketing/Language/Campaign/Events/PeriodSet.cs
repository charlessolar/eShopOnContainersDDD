using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Events
{
    public interface PeriodSet : IStampedEvent
    {
        Guid CampaignId { get; set; }

        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}
