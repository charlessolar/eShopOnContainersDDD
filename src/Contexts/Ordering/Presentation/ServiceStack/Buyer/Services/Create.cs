using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Services
{
    [Api("Ordering")]
    [Route("/buyer", "POST")]
    public class CreateBuyer : DomainCommand
    {
        public Guid BuyerId { get; set; }

        public string GivenName { get; set; }
    }
}
