﻿using Infrastructure.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Operation : Enumeration<Operation, string>
    {

        public static Operation Equal = new Operation("EQ", "Equal");
        public static Operation NotEqual = new Operation("NE", "Not Equal");
        public static Operation GreaterThan = new Operation("GT", "Greater Than");
        public static Operation GreaterThanOrEqual = new Operation("GTE", "Greater Than or Equal");
        public static Operation LessThan = new Operation("LT", "Less Than");
        public static Operation LessThanOrEqual = new Operation("LTE", "Less Than or Equal");
        public static Operation Contains = new Operation("CN", "Contains");
        public static Operation Autocomplete = new Operation("AC", "Autocomplete");

        public Operation(string value, string displayName) : base(value, displayName) { }
    }
}
