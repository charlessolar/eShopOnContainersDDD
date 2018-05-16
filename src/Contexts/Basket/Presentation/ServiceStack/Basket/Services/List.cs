using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Basket.Basket.Services
{
    [Api("Basket")]
    [Route("/basket/list", "GET")]
    public class ListBaskets : Paged<Models.BasketIndex>
    {
    }
}
