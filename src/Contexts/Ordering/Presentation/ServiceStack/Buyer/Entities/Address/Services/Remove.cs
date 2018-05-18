using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.Address.Services
{
    [Api("Ordering")]
    [Route("/buyer/address/{AddressId}", "DELETE")]
    public class RemoveBuyerAddress : DomainCommand
    {
        public Guid AddressId { get; set; }
    }
}
