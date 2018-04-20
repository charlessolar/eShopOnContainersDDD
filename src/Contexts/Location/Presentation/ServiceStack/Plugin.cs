using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Location
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Location.Service>();
            appHost.RegisterService<Location.Entities.Point.Service>();
            appHost.RegisterService<User.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(Location.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Location.Entities.Point.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(User.Service));
        }
    }
}
