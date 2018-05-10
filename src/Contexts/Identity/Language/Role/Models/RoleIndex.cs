using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.Role.Models
{
    public class RoleIndex
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int Users { get; set; }

        public bool Disabled { get; set; }
    }
}
