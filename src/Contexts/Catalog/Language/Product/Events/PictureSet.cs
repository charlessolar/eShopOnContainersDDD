using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Events
{
    public interface PictureSet : IStampedEvent
    {
        Guid ProductId { get; set; }

        byte[] Content { get; set; }
        string ContentType { get; set; }
    }
}
