using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Queries
{

    // Used when replying to a query
    public class Reply : Aggregates.Messages.IMessage
    {
        public long ElapsedMs { get; set; }

        public string ETag { get; set; }

        public object Payload { get; set; }
    }
    public class PagedReply : Aggregates.Messages.IMessage
    {
        public long ElapsedMs { get; set; }

        public long Total { get; set; }

        public IEnumerable<object> Records { get; set; }
    }
}
