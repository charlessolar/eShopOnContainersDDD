using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Ordering.Types
{
    public class Address
    {
        public Guid AddressId { get; set; }

        public string Alias { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
