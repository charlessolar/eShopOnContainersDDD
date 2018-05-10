using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Queries;

namespace eShop.Identity.User.Queries
{
    public class Identity : Query
    {
        public string UserName { get; set; }
    }
}
