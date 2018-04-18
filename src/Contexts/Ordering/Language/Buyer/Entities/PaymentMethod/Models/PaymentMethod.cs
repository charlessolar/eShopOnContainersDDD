using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Models
{
    public class PaymentMethod
    {
        public Guid BuyerId { get; set; }
        public Guid PaymentMethodId { get; set; }

        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardholderName { get; set; }

        public DateTime Expiration { get; set; }

        public CardType CardType { get; set; }
    }
}
