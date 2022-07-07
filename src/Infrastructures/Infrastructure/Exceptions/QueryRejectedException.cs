using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class QueryRejectedException : Exception
    {
        public QueryRejectedException() { }
        public QueryRejectedException(string message) : base(message) { }
    }
}
