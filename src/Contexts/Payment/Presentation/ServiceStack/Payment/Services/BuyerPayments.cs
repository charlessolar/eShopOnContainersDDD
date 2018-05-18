using Infrastructure.ServiceStack;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment.Services
{
    [Api("Payment")]
    [Route("/payment", "GET")]
    public class BuyerPayments : Paged<Models.PaymentIndex>
    {
    }
}
