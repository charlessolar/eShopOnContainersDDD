using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Events
{
    public interface PictureSet : IStampedEvent
    {
        Guid CampaignId { get; set; }

        string Content { get; set; }
        string ContentType { get; set; }
    }
}
