using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Commands
{
    public class Disable : StampedCommand
    {
        public Guid UserId { get; set; }
    }
}
