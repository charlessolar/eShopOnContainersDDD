using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    [Api("Catalog")]
    [Route("/catalog/products/{ProductId}/picture", "POST")]
    public class SetPictureProduct : DomainCommand
    {
        public Guid ProductId { get; set; }

        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
