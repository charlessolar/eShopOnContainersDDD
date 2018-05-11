using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Catalog.CatalogBrand
{
    public class Brand : Aggregates.Entity<Brand, State>
    {
        private Brand() { }

        public void Define(string brand)
        {
            Apply<Events.Defined>(x =>
            {
                x.BrandId = Id;
                x.Brand = brand;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(x =>
            {
                x.BrandId = Id;
            });
        }
    }
}
