using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Responses
{
    public class Paged<TResponse>
    {
        public long ElapsedMs { get; set; }
        public long Total { get; set; }
        public TResponse[] Records { get; set; }
    }
}
