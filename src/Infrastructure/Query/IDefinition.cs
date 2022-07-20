using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public interface IDefinition
    {
        IGrouped[] Operations { get; set; }

        long? Skip { get; set; }
        long? Take { get; set; }
        ISort[] Sort { get; set; }
    }
}

