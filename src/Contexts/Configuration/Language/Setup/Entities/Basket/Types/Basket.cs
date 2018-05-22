using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Basket.Types
{
    public class Basket
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public Tuple<Guid, long>[] Products { get; set; }
    }
}
