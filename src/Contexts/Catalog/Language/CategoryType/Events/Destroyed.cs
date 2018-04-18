using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CategoryType.Events
{
    public interface Destroyed : IStampedEvent
    {
        Guid TypeId { get; set; }
    }
}
