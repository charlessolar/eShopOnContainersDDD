using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException(string message) : base(message) { }
    }
}
