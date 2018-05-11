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
            appHost.RegisterService<CatalogBrand.Service>();
            appHost.RegisterService<CatalogType.Service>();
            appHost.RegisterService<Product.Service>();

            appHost.GetContainer().RegisterAutoWiredType(typeof(CatalogBrand.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(CatalogType.Service));
            appHost.GetContainer().RegisterAutoWiredType(typeof(Product.Service));
        }
    }
}
