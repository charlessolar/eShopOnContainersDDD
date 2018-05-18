using System;
using ServiceStack;
using Infrastructure.ServiceStack;

namespace eShop.Payment.Payment.Services
{
    [Api("Payment")]
    [Route("/payment", "POST")]
    public class Charge : DomainCommand
    {
        public Guid PaymentId { get; set; }

        public Guid OrderId { get; set; }
    }
}
