using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;


namespace eShop.Marketing.Campaign.Services
{
    [Api("Marketing")]
    [Route("/campaign", "POST")]
    public class DefineCampaign : DomainCommand
    {
        public Guid CampaignId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
