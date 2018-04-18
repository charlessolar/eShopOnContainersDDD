using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Marketing.Campaign.Entities.Rule.Models
{
    public class Rule
    {
        public Guid CampaignId { get; set; }
        public Guid RuleId { get; set; }

        public string Description { get; set; }
    }
}
