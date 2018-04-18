using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Entities.Address.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid BuyerId { get; set; }

        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Country { get; set; }
        public String ZipCode { get; set; }
    }
}
