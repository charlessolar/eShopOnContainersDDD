using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.Address.Events
{
    public interface Added : IStampedEvent
    {
        Guid BuyerId { get; set; }
        Guid AddressId { get; set; }

        String Street { get; set; }
        String City { get; set; }
        String State { get; set; }
        String Country { get; set; }
        String ZipCode { get; set; }
    }
}
