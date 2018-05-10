using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.User.Models
{
    public class User
    {
        public string Id { get; set; }
        public string GivenName { get; set; }

        public bool Disabled { get; set; }

        public string[] Roles { get; set; }

        public long LastLogin { get; set; }
    }
}
