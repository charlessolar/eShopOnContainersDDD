using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CatalogType.Events
{
    public interface Defined  : IStampedEvent
    {
        Guid TypeId { get; set; }
        string Type { get; set; }
    }
}
