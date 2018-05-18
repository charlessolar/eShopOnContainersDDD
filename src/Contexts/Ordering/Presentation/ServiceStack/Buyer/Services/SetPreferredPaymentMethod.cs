using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Services
{
    [Api("Ordering")]
    [Route("/buyer/{UserName}/preferred_payment", "POST")]
    public class SetPreferredPaymentMethod : DomainCommand
    {
        public string UserName { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
