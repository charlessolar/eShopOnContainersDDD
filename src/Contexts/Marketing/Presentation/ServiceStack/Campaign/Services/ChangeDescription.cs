using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;


namespace eShop.Marketing.Campaign.Services
{
    [Api("Marketing")]
    [Route("/campaign/{CampaignId}/description", "POST")]
    public class ChangeDescriptionCampaign : DomainCommand
    {
        public Guid CampaignId { get; set; }
        public string Description { get; set; }
    }
}
