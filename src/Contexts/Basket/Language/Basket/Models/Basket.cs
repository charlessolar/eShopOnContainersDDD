using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eShop.Basket.Basket.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public string CustomerId { get; set; }
        public string Customer { get; set; }

        public int TotalItems => Items?.Count() ?? 0;
        public long TotalQuantity => Items?.Sum(x => x.Quantity) ?? 0L;

        public long SubTotal => Items?.Sum(x => x.SubTotal) ?? 0L;

        public long Created { get; set; }
        public long Updated { get; set; }

        public Entities.Item.Models.BasketItem[] Items { get; set; }
    }
}
