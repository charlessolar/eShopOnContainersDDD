using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Marketing.Campaign.Entities.Rule
{
    public class Handler :
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.Remove>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.For<Campaign>().Get(command.CampaignId).ConfigureAwait(false);
            var rule = await campaign.For<Rule>().New(command.RuleId).ConfigureAwait(false);
            rule.Add(command.Description);
        }

        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.For<Campaign>().Get(command.CampaignId).ConfigureAwait(false);
            var rule = await campaign.For<Rule>().Get(command.RuleId).ConfigureAwait(false);
            rule.Remove();
        }
    }
}
