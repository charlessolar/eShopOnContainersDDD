using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Basket.Basket.Entities.Item.Events
{
    public interface ItemRemoved : IStampedEvent
    {
        public Guid BasketId { get; set; }
        public Guid ItemId { get; set; }
    }
}
