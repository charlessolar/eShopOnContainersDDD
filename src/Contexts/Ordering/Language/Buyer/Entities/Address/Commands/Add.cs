using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.Address.Commands
{
    public class Add : StampedCommand
    {
        public Guid BuyerId { get;set; }
        public Guid AddressId { get; set; }

        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Country { get; set; }
        public String ZipCode { get; set; }
    }
}
