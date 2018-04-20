using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Basket.Basket.Services
{
    [Api("Basket")]
    [Route("/basket", "DELETE")]
    public class BasketDestroy : Command
    {
        public Guid BasketId { get; set; }
    }
}
