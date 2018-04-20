using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Order.Services
{
    [Api("Ordering")]
    [Route("/order/{OrderId}/pay", "POST")]
    public class PayOrder :DomainCommand
    {
        public Guid OrderId { get; set; }
    }
}
