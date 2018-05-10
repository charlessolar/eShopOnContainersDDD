using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.Role.Commands
{
    public class Revoke : StampedCommand
    {
        public Guid RoleId { get; set; }
    }
}
