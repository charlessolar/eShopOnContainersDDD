using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string Status { get; set; }
        public string StatusDescription { get; set; }

        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; }

        public Guid AddressId { get; set; }
        public string Address { get; set; }
        public string CityState { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public Guid PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }

        public decimal Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
