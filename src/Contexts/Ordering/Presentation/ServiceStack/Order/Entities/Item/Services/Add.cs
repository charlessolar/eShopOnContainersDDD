using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Order.Entities.Item.Services
{
    [Api("Ordering")]
    [Route("/order/{OrderId}/item", "POST")]
    public class AddOrderItem : DomainCommand
    {
        public Guid ItemId { get; set; }
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }
    }
}
