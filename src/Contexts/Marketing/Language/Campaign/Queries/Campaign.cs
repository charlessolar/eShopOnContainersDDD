using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Marketing.Campaign.Queries
{
    public class Campaign : Query
    {
        public Guid CampaignId { get; set; }
    }
}
