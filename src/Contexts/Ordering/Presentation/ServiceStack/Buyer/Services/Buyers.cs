using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Buyer.Services
{
    [Api("Ordering")]
    [Route("/buyers", "GET")]
    public class Buyers : Paged<Models.BuyerIndex>
    {
    }
}
