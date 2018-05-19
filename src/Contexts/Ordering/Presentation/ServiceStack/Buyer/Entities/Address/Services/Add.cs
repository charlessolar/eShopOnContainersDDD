using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.Address.Services
{
    [Api("Ordering")]
    [Route("/buyer/address", "POST")]
    public class AddBuyerAddress : DomainCommand
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
