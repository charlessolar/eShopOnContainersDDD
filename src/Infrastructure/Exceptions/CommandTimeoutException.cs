using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class CommandTimeoutException : Exception
    {
        public CommandTimeoutException()
        {
        }

        public CommandTimeoutException(string message)
            : base(message)
        {
        }
    }
}
