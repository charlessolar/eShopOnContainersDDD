using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment
{
    public class State : Aggregates.State<State>
    {
        public Status Status { get; private set; }


    }
}
