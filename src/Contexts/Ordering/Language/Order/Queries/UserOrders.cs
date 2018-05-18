using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Order.Queries
{
    public class UserOrders : Paged
    {
        public string UserName { get; set; }
    }
}
