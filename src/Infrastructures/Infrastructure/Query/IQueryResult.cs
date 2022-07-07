using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public interface IQueryResult<T> where T : class
    {
        T[] Records { get; set; }
        long Total { get; set; }
        long ElapsedMs { get; set; }
    }
}