using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Events
{
    public interface Added : IStampedEvent
    {
        Guid ProductId { get; set; }

        string Name { get; set; }
        int Price { get; set; }

        Guid CatalogBrandId { get; set; }
        Guid CatalogTypeId { get; set; }
    }
}
