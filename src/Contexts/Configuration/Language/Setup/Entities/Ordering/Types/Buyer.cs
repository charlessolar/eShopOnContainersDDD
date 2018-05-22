using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Ordering.Types
{
    public class Buyer
    {
        public string UserName { get; set; }
        public string GivenName { get; set; }
        public Address Address { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
