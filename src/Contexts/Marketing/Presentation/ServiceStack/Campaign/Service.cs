using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Marketing.Campaign
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.GetCampaign request)
        {
            return _bus.RequestQuery<Queries.Campaign, Models.Campaign>(new Queries.Campaign
            {
                CampaignId = request.CampaignId
            });
        }
        public Task<object> Any(Services.ListCampaigns request)
        {
            return _bus.RequestQuery<Queries.Campaign, Models.Campaign>(new Queries.Campaign
            {
            });
        }

        public Task Any(Services.ChangeDescriptionCampaign request)
        {
            return _bus.CommandToDomain(new Commands.ChangeDescription
            {
                CampaignId = request.CampaignId,
                Description = request.Description
            });
        }

        public Task Any(Services.DefineCampaign request)
        {
            return _bus.CommandToDomain(new Commands.Define
            {
                CampaignId = request.CampaignId,
                Description = request.Description,
                Name = request.Name
            });
        }

        public Task Any(Services.SetPeriodCampaign request)
        {
            return _bus.CommandToDomain(new Commands.SetPeriod
            {
                CampaignId = request.CampaignId,
                Start = request.Start,
                End = request.End
            });
        }

        public async Task Any(Services.SetPictureCampaign request)
        {
            var image = await GetImageFromUrl(request.PictureUrl).ConfigureAwait(false);

            await _bus.CommandToDomain(new Commands.SetPicture
            {
                CampaignId = request.CampaignId,
                Content = Convert.ToBase64String(image.Data),
                ContentType = image.Type
            }).ConfigureAwait(false);
        }

        private class Image
        {
            public string Type { get; set; }

            public byte[] Data { get; set; }
        }

        private async Task<Image> GetImageFromUrl(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                var response = await request.GetResponseAsync().ConfigureAwait(false);

                var contentType = response.ContentType;
                var buffer = response.GetResponseStream().ReadFully();
                response.Close();

                return new Image { Type = contentType, Data = buffer };
            }
            catch
            {
                return null;
            }
        }
    }
}
