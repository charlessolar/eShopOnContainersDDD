using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Identity
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<User.Service>();
            appHost.RegisterService<Role.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(User.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Role.Service));
        }
    }
}
