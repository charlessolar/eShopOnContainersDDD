using System;
using System.Collections.Generic;
using System.Linq;
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

        public Guid ShippingAddressId { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCityState { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingCountry { get; set; }
        public Guid BillingAddressId { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCityState { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingCountry { get; set; }

        public Guid PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }

        public int TotalItems => Items.Count();
        public long TotalQuantity => Items.Sum(x => x.Quantity);

        public long SubTotal => Items.Sum(x => x.SubTotal);

        public long AdditionalFees { get; set; }
        public long AdditionalTaxes { get; set; }

        public long Total => SubTotal + AdditionalFees + AdditionalTaxes;

        public long Created { get; set; }
        public long Updated { get; set; }

        public bool Paid { get; set; }

        public Entities.Item.Models.OrderingOrderItem[] Items { get; set; }
    }
}
