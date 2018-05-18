using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Services
{
    [Api("Ordering")]
    [Route("/buyer/{UserName}/good", "POST")]
    public class MarkGoodStanding : DomainCommand
    {
        public string UserName { get; set; }
    }
}
