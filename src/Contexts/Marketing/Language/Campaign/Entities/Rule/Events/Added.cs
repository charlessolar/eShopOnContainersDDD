using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Entities.Rule.Events
{
    public interface Added : IStampedEvent
    {
        Guid CampaignId { get; set; }
        Guid RuleId { get; set; }

        string Description { get; set; }
    }
}
