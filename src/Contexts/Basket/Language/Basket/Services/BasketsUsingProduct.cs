using System;
using System.Collections.Generic;
using System.Text;
using Aggregates;

namespace eShop.Basket.Basket.Services
{
    public class BasketsUsingProduct : IService<Guid[]>
    {
        public Guid ProductId { get; set; }
    }
}
