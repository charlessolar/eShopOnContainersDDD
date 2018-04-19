using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Marketing.Campaign.Entities.Rule
{
    public class Rule : Aggregates.Entity<Rule, State, Campaign>
    {
        private Rule() { }

        public void Add(string description)
        {
            Apply<Events.Added>(x =>
            {
                x.CampaignId = Parent.Id;
                x.RuleId = Id;
                x.Description = description;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.CampaignId = Parent.Id;
                x.RuleId = Id;
            });
        }
    }
}
