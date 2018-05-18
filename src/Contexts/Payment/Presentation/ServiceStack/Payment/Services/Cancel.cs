using System;
using ServiceStack;
using Infrastructure.ServiceStack;

namespace eShop.Payment.Payment.Services
{
    [Api("Payment")]
    [Route("/payment/{PaymentId}/cancel", "POST")]
    public class Cancel : DomainCommand
    {
        public Guid PaymentId { get; set; }
    }
}
