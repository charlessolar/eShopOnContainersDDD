using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Order.Services
{
    [Api("Ordering")]
    [Route("/order/{OrderId}", "GET")]
    public class GetOrder : Query<Models.OrderingOrder>
    {
        public Guid OrderId { get; set; }
    }
}
