using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Catalog.Product
{
    public class State : Aggregates.State<State>
    {
        public bool ReorderMarked { get; private set; }

        private void Handle(Events.ReorderMarked e)
        {
            ReorderMarked = true;
        }
        private void Handle(Events.ReorderUnMarked e)
        {
            ReorderMarked = false;
        }
    }
}
