using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Basket.Basket.Events
{
    public interface BasketClaimed : IStampedEvent
    {
        Guid BasketId { get; set; }
        string UserName { get; set; }
    }
}
