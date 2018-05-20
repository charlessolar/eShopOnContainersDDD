using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Ordering.Order.Services
{
    [Api("Ordering")]
    [Route("/order/{OrderId}/address", "POST")]
    public class ChangeAddressOrder : DomainCommand
    {
        public Guid OrderId { get; set; }
        public Guid ShippingId { get; set; }
        public Guid BillingId { get; set; }
    }
}
