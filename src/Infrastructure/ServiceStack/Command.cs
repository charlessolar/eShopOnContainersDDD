using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace Infrastructure.ServiceStack
{
    public class Command : IReturn<CommandResponse>, IReturn
    {
    }
    public class CommandResponse
    {
        public long RoundTripMs { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
