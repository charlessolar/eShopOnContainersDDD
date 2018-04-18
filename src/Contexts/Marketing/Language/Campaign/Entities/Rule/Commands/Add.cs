using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Entities.Rule.Commands
{
    public class Add : StampedCommand
    {
        public Guid CampaignId { get; set; }
        public Guid RuleId { get; set; }

        public string Description { get; set; }
    }
}
