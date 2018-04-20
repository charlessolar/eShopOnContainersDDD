using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Marketing.Campaign
{
    public class Handler :
        IHandleMessages<Commands.Define>,
        IHandleMessages<Commands.ChangeDescription>,
        IHandleMessages<Commands.SetPeriod>,
        IHandleMessages<Commands.SetPicture>
    {
        public async Task Handle(Commands.Define command, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.For<Campaign>().New(command.CampaignId).ConfigureAwait(false);
            campaign.Define(command.Name, command.Description);
        }

        public async Task Handle(Commands.ChangeDescription command, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.For<Campaign>().Get(command.CampaignId).ConfigureAwait(false);
            campaign.ChangeDescription(command.Description);
        }

        public async Task Handle(Commands.SetPeriod command, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.For<Campaign>().Get(command.CampaignId).ConfigureAwait(false);
            campaign.SetPeriod(command.Start, command.End);
        }
        public async Task Handle(Commands.SetPicture command, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.For<Campaign>().Get(command.CampaignId).ConfigureAwait(false);
            campaign.SetPicture(command.Content, command.ContentType);
        }

    }
}
