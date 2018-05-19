using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.Address.Commands
{
    public class Add : StampedCommand
    {
        public string UserName { get;set; }
        public Guid AddressId { get; set; }

        public string Alias { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
