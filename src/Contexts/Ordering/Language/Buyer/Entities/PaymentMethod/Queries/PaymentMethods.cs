using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Queries
{
    public class PaymentMethods : Paged
    {
        public string UserName { get; set; }

        public string Term { get; set; }
        public Guid? Id { get; set; }
    }
}
