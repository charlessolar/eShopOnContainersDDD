using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Basket
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Basket.Service>();
            appHost.RegisterService<Basket.Entities.Item.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(Basket.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Basket.Entities.Item.Service));
        }
    }
}
