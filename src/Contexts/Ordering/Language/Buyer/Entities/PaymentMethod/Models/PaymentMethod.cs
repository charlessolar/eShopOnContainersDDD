using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Models
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }
        public Guid BuyerId { get; set; }

        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardholderName { get; set; }

        public DateTime Expiration { get; set; }

        public string CardType { get; set; }
    }
}
