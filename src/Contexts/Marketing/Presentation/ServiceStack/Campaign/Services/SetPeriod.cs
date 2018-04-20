using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;


namespace eShop.Marketing.Campaign.Services
{
    [Api("Marketing")]
    [Route("/campaign/{CampaignId}/period", "POST")]
    public class SetPeriodCampaign : DomainCommand
    {
        public Guid CampaignId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
