using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Commands
{
    public class ChangeName : StampedCommand
    {
        public string UserName { get; set; }
        public string GivenName { get; set; }
    }
}
