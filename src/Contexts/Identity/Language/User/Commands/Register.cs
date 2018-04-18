using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Commands
{
    public class Register : StampedCommand
    {
        public Guid UserId { get; set; }
        public string GivenName { get; set; }
    }
}
