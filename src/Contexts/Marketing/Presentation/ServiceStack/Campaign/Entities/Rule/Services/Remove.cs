using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;


namespace eShop.Marketing.Campaign.Entities.Rule.Services
{
    [Api("Marketing")]
    [Route("/campaign/{CampaignId}/rule/{RuleId}", "DELETE")]
    public class RemoveCampaignRule : DomainCommand
    {
        public Guid CampaignId { get; set; }
        public Guid RuleId { get; set; }
    }
}
