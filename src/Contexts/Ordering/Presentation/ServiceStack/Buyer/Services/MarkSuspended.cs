using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Services
{
    [Api("Ordering")]
    [Route("/buyer/{UserName}/suspend", "POST")]
    public class MarkSuspended : DomainCommand
    {
        public string UserName { get; set; }
    }
}
