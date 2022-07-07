using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Queries
{
    public interface IHandleQueries<TQuery> : IHandleMessages<TQuery> where TQuery : Query
    {
    }
}
