using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Ordering.Types
{
    public class PaymentMethod
    {
        public Guid PaymentMethodId { get; set; }

        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardholderName { get; set; }

        public DateTime Expiration { get; set; }

        public eShop.Ordering.Buyer.Entities.PaymentMethod.CardType CardType { get; set; }
    }
}
