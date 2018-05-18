using Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment.Queries
{
    public class BuyerPayments : Paged
    {
        public string UserName { get; set; }
    }
}
