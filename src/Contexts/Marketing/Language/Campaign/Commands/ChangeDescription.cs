using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Commands
{
    public class ChangeDescription : StampedCommand
    {
        public Guid CampaignId { get; set; }

        public string Description { get; set; }
    }
}
