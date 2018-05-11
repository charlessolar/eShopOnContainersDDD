using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Catalog.CatalogType
{
    public class Type : Aggregates.Entity<Type, State>
    {
        private Type() { }

        public void Define(string type)
        {
            Apply<Events.Defined>(x =>
            {
                x.TypeId = Id;
                x.Type = type;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(x =>
            {
                x.TypeId = Id;
            });
        }
    }
}
