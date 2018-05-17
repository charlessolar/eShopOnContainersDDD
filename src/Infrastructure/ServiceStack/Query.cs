using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace Infrastructure.ServiceStack
{
    public class Query<T> : IReturn<QueryResponse<T>>
    {
    }
    public class QueryResponse<T>
    {
        public long RoundTripMs { get; set; }
        public T Payload { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class Paged<T> : IReturn<PagedResponse<T>>
    {
    }

    public class PagedResponse<T>
    {
        public long RoundTripMs { get; set; }
        public long Total { get; set; }
        public T[] Records { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
