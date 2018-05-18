using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Models
{
    public class OrderingOrder
    {
        public Guid Id { get; set; }

        public string Status { get; set; }
        public string StatusDescription { get; set; }

        public string UserName { get; set; }
        public string BuyerName { get; set; }

        public Guid AddressId { get; set; }
        public string Address { get; set; }
        public string CityState { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public Guid PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }

        public int TotalItems { get; set; }
        public long TotalQuantity { get; set; }

        public long SubTotal { get; set; }

        public long AdditionalFees { get; set; }
        public long AdditionalTaxes { get; set; }

        public long Total => SubTotal + AdditionalFees + AdditionalTaxes;

        public bool Paid { get; set; }
    }
}
