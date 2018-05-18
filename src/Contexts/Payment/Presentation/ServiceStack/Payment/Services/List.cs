using System;
using ServiceStack;
using Infrastructure.ServiceStack;

namespace eShop.Payment.Payment.Services
{
    [Api("Payment")]
    [Route("/payments", "GET")]
    public class PaymentList : Paged<Models.PaymentIndex>
    {
    }
}
