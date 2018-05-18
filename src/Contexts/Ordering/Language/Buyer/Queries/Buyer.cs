using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Ordering.Buyer.Queries
{
    public class Buyer : Query
    {
        public string UserName { get; set; }
    }
}
