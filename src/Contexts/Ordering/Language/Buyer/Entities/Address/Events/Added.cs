using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.Address.Events
{
    public interface Added : IStampedEvent
    {
        string UserName { get; set; }
        Guid AddressId { get; set; }

        string Alias { get; set; }
        string Street { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Country { get; set; }
        string ZipCode { get; set; }
    }
}
