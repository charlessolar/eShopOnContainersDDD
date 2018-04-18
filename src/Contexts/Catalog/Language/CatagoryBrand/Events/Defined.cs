using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CatagoryBrand.Events
{
    public interface Defined : IStampedEvent
    {
        Guid BrandId { get; set; }
        string Brand { get; set; }
    }
}
