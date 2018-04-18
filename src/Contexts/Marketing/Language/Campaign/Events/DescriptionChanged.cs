using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Events
{
    public interface DescriptionChanged : IStampedEvent
    {
        Guid CampaignId { get; set; }

        string Description { get; set; }
    }
}
