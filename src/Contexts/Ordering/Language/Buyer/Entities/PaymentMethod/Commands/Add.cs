using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod.Commands
{
    public class Add : StampedCommand
    {
        public string UserName { get; set; }
        public Guid PaymentMethodId { get; set; }

        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardholderName { get; set; }

        public DateTime Expiration { get; set; }

        public CardType CardType { get; set; }
    }
}
