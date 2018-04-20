using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;


namespace eShop.Marketing.Campaign.Services
{
    [Api("Marketing")]
    [Route("/campaign/{CampaignId}/picture", "POST")]
    public class SetPictureCampaign : DomainCommand
    {
        public Guid CampaignId { get; set; }

        public string PictureUrl { get; set; }
    }
}
