using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Marketing
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Campaign.Service>();
            appHost.RegisterService<Campaign.Entities.Rule.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(Campaign.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Campaign.Entities.Rule.Service));
        }
    }
}
