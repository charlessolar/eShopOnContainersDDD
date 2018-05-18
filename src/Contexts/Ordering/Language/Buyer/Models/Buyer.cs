using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Models
{
    public class Buyer
    {
        public string UserName { get; set; }
        public string GivenName { get; set; }

        public bool GoodStanding { get; set; }

        public Guid? PreferredAddressId { get; set; }
        public Guid? PreferredPaymentMethodId { get; set; }
    }
}
