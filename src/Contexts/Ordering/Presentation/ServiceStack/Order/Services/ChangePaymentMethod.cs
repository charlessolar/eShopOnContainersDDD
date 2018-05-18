using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Order.Services
{
    [Api("Ordering")]
    [Route("/order/{OrderId}/payment_method", "POST")]
    public class ChangePaymentMethodOrder : DomainCommand
    {
        public Guid OrderId { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
