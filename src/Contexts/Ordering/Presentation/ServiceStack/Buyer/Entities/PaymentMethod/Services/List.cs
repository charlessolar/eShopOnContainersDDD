using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Services
{
    [Api("Ordering")]
    [Route("/buyer/{BuyerId}/payment_method", "GET")]
    public class ListPaymentMethods : Paged<Models.PaymentMethod>
    {
        public Guid BuyerId { get; set; }
    }
}
