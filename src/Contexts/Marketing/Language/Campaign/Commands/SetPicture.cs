using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Commands
{
    public class SetPicture : StampedCommand
    {
        public Guid CampaignId { get; set; }

        public string PictureUrl { get; set; }
    }
}
