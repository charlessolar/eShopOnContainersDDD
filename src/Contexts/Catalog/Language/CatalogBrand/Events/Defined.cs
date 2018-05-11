using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CatalogBrand.Events
{
    public interface Defined : IStampedEvent
    {
        Guid BrandId { get; set; }
        string Brand { get; set; }
    }
}
