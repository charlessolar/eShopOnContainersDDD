using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Catalog.Product.Services
{
    public class ListProducts : Paged<Models.ProductIndex>
    {
    }
}
