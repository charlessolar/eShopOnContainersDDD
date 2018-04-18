using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Commands
{
    public class SetPeriod : StampedCommand
    {
        public Guid CampaignId { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
