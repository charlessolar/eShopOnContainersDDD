using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Services
{
    [Api("Ordering")]
    [Route("/buyer/{BuyerId}/payment_method", "POST")]
    public class AddBuyerPaymentMethod : DomainCommand
    {
        public Guid BuyerId { get; set; }
        public Guid PaymentMethodId { get; set; }

        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardholderName { get; set; }

        public DateTime Expiration { get; set; }

        public string CardType { get; set; }
    }
}
