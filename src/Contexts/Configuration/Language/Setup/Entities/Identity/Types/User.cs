using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Identity.Types
{
    public class User
    {
        public string UserName { get; set; }
        public string GivenName { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}
