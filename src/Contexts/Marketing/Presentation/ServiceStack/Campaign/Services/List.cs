using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Marketing.Campaign.Services
{
    public class ListCampaigns : Paged<Models.Campaign>
    {
    }
}
