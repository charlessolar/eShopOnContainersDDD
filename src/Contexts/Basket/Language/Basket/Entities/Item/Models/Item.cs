using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket.Entities.Item.Models
{
    public class BasketItem
    {
        public string Id { get; set; }
    
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }

        public string ProductPictureContents { get; set; }
        public string ProductPictureContentType { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public long ProductPrice { get; set; }
        
        public long Quantity { get; set; }

        public long SubTotal => ProductPrice * Quantity;
        public long AdditionalTaxes { get; set; }
        public long AdditionalFees { get; set; }

        public long Total => SubTotal + AdditionalTaxes + AdditionalFees;
    }
}
