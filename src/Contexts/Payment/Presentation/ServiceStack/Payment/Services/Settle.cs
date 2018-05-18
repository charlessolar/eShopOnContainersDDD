using System;
using ServiceStack;
using Infrastructure.ServiceStack;

namespace eShop.Payment.Payment.Services
{
    [Api("Payment")]
    [Route("/payment/{PaymentId}/settle", "POST")]
    public class Settle : DomainCommand
    {
        public Guid PaymentId { get; set; }
    }
}
