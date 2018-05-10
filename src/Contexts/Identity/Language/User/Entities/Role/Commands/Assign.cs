using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Entities.Role.Commands
{
    public class Assign : StampedCommand
    {
        public string UserName { get; set; }
        public Guid RoleId { get; set; }
    }
}
