using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.Address.Services
{
    [Api("Ordering")]
    [Route("/buyer/{BuyerId}/address", "GET")]
    public class ListAddresses : Paged<Models.Address>
    {
        public Guid BuyerId { get; set; }
    }
}
