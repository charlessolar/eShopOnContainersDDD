using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.CategoryBrand.Events
{
    public interface Destroyed : IStampedEvent
    {
        Guid BrandId { get; set; }
    }
}
