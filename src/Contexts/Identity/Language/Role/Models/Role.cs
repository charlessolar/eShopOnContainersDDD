using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.Role.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool Disabled { get; set; }
    }
}
