using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace eShop.Catalog
{
    public class Plugin : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            appHost.RegisterService<CategoryBrand.Service>();
            appHost.RegisterService<CategoryType.Service>();
            appHost.RegisterService<Product.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(CategoryBrand.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(CategoryType.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Product.Service));
        }
    }
}
