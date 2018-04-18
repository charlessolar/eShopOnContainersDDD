using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Marketing.Campaign.Models
{
    public class Campaign
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

    }
}
