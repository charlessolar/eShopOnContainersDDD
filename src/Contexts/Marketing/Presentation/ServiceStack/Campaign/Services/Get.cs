using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Marketing.Campaign.Services
{
    public class GetCampaign : Query<Models.Campaign>
    {
        public Guid CampaignId { get; set; }
    }
}
