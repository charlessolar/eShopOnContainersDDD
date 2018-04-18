using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Entities.Rule.Events
{
    public interface Removed : IStampedEvent
    {
        Guid CampaignId { get; set; }
        Guid RuleId { get; set; }
    }
}
