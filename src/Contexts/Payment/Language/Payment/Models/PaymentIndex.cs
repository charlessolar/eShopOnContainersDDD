using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment.Models
{
    public class PaymentIndex
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string GivenName { get; set; }

        public string Status { get; set; }
        public string StatusDescription { get; set; }

        public Guid OrderId { get; set; }
        public string Reference { get; set; }

        public long TotalPayment { get; set; }

        public string PaymentMethodCardholder { get; set; }
        public string PaymentMethodMethod { get; set; }
        
        public long Created { get; set; }
        public long Updated { get; set; }
        public long? Settled { get; set; }
    }
}
