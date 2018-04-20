using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Ordering
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<Buyer.Service>();
            appHost.RegisterService<Buyer.Entities.Address.Service>();
            appHost.RegisterService<Buyer.Entities.PaymentMethod.Service>();
            appHost.RegisterService<Order.Service>();
            appHost.RegisterService<Order.Entities.Item.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(Buyer.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Buyer.Entities.Address.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Buyer.Entities.PaymentMethod.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Order.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Order.Entities.Item.Service));
        }
    }
}
