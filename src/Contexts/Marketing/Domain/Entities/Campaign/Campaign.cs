using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Marketing.Campaign
{
    public class Campaign : Aggregates.Entity<Campaign, State>
    {
        private Campaign() { }

        public void Define(string name, string description)
        {
            Apply<Events.Defined>(x =>
            {
                x.CampaignId = Id;
                x.Name = name;
                x.Description = description;
            });
        }

        public void ChangeDescription(string description)
        {
            Apply<Events.DescriptionChanged>(x =>
            {
                x.CampaignId = Id;
                x.Description = description;
            });
        }

        public void SetPeriod(DateTime start, DateTime end)
        {
            Apply<Events.PeriodSet>(x =>
            {
                x.CampaignId = Id;
                x.From = start;
                x.To = end;
            });
        }

        public void SetPicture(byte[] content, string contentType)
        {
            Apply<Events.PictureSet>(x =>
            {
                x.CampaignId = Id;
                x.Content = content;
                x.ContentType = contentType;
            });
        }

    }
}
