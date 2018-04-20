using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Marketing.Campaign.Entities.Rule
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task Any(Services.AddCampaignRule request)
        {
            return _bus.CommandToDomain(new Commands.Add
            {
                CampaignId = request.CampaignId,
                RuleId = request.RuleId,
                Description = request.Description
            });
        }

        public Task Any(Services.RemoveCampaignRule request)
        {
            return _bus.CommandToDomain(new Commands.Remove
            {
                CampaignId = request.CampaignId,
                RuleId = request.RuleId
            });
        }
    }
}
