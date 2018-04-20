using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace Infrastructure.ServiceStack
{
    public class DomainCommand : IReturn<CommandResponse>, IReturn
    {
    }
    public class CommandResponse
    {
        public long RoundTripMs { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
