using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Configuration
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Setup.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(Setup.Service));
        }
    }
}
