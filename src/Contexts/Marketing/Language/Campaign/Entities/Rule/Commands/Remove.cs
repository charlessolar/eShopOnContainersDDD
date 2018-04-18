using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Entities.Rule.Commands
{
    public class Remove :StampedCommand
    {
        public Guid CampaignId { get; set; }
        public Guid RuleId { get; set; }
    }
}
