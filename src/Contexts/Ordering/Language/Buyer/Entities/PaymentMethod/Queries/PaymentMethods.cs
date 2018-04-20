using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Queries
{
    public class PaymentMethods : Paged
    {
        public Guid BuyerId { get; set; }
    }
}
