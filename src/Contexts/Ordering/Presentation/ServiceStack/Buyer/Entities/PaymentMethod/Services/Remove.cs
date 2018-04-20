using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Services
{
    [Api("Ordering")]
    [Route("/buyer/{BuyerId}/payment_method/{PaymentMethodId}", "DELETE")]
    public class RemoveBuyerPaymentMethod : DomainCommand
    {
        public Guid BuyerId { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
