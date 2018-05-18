using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Models
{
    public class OrderingBuyerIndex
    {
        public string Id { get; set; }
        public string GivenName { get; set; }

        public bool GoodStanding { get; set; }

        public long TotalSpent { get; set; }

        public string PreferredCity { get; set; }
        public string PreferredState { get; set; }
        public string PreferredCountry { get; set; }
        public string PreferredZipCode { get; set; }

        public string PreferredPaymentCardholder { get; set; }
        public string PreferredPaymentMethod { get; set; }
        public string PreferredPaymentExpiration { get; set; }
    }
}
