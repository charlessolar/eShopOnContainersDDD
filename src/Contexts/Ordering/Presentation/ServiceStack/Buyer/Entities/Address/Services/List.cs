using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Entities.Address.Services
{
    [Api("Ordering")]
    [Route("/buyer/address", "GET")]
    public class ListAddresses : Paged<Models.Address>
    {
    }
}
