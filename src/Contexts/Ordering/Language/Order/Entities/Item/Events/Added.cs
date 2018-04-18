﻿using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Entities.Item.Events
{
    public interface Added : IStampedEvent
    {
        Guid ItemId { get; set; }
        Guid OrderId { get; set; }

        Guid ProductId { get; set; }

        decimal Quantity { get; set; }
    }
}
