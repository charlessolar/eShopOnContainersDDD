using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Buyer.Entities.Address.Queries
{
    public class Addresses : Paged
    {
        public Guid BuyerId { get; set; }
    }
}
