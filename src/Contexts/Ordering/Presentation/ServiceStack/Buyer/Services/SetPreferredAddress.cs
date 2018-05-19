using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Services
{
    [Api("Ordering")]
    [Route("/buyer/{UserName}/preferred_address", "POST")]
    public class SetPreferredAddress : DomainCommand
    {
        public Guid AddressId { get; set; }
    }
}
