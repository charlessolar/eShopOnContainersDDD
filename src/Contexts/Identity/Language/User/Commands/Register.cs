﻿using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Commands
{
    public class Register : StampedCommand
    {
        public string GivenName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
