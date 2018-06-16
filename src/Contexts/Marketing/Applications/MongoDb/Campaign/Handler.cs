using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Marketing.Campaign
{
    public class Handler :
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.DescriptionChanged>,
        IHandleMessages<Events.PeriodSet>,
        IHandleMessages<Events.PictureSet>
    {
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.Campaign
            {
                Id = e.CampaignId,
                Description = e.Description,
                Name = e.Name
            };
            return ctx.UoW().Add(e.CampaignId, model);
        }

        public async Task Handle(Events.DescriptionChanged e, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.UoW().Get<Models.Campaign>(e.CampaignId)
                .ConfigureAwait(false);

            campaign.Description = e.Description;

            await ctx.UoW().Update(e.CampaignId, campaign).ConfigureAwait(false);
        }

        public async Task Handle(Events.PeriodSet e, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.UoW().Get<Models.Campaign>(e.CampaignId)
                .ConfigureAwait(false);

            campaign.Start = e.Start;
            campaign.End = e.End;

            await ctx.UoW().Update(e.CampaignId, campaign).ConfigureAwait(false);
        }
        public async Task Handle(Events.PictureSet e, IMessageHandlerContext ctx)
        {
            var campaign = await ctx.UoW().Get<Models.Campaign>(e.CampaignId)
                .ConfigureAwait(false);

            campaign.PictureContents = e.Content;
            campaign.PictureContentType = e.ContentType;

            await ctx.UoW().Update(e.CampaignId, campaign).ConfigureAwait(false);
        }
    }
}
