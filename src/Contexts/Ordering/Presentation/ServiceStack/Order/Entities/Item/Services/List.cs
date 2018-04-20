using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Order.Entities.Item.Services
{
    [Api("Ordering")]
    [Route("/order/{OrderId}/item", "GET")]
    public class ListOrderItems : Paged<Models.Item>
    {
        public Guid OrderId { get; set; }
    }
}
