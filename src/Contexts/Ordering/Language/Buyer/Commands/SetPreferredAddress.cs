using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Commands
{
    public class SetPreferredAddress : StampedCommand
    {
        public string UserName { get; set; }
        public Guid AddressId { get; set; }
    }
}
