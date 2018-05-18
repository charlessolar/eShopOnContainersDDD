using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Entities.Item.Models
{
    public class OrderingOrderItem
    {
        public string Id { get; set; }
        public Guid OrderId { get; set;}

        public Guid ProductId { get; set; }
        public string ProductPictureContents { get; set; }
        public string ProductPictureContentType { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public long ProductPrice { get; set; }
        public long? Price { get; set; }

        public long Quantity { get; set; }

        public long SubTotal => (Price ?? ProductPrice) * Quantity;
        public long AdditionalFees { get; set; }
        public long AdditionalTaxes { get; set; }

        public long Total => SubTotal + AdditionalFees + AdditionalTaxes;
    }
}
