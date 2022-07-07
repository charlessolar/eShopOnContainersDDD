using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public interface IFieldDefinition
    {
        string Field { get; set; }

        string Value { get; set; }

        string Op { get; set; }

        double? Boost { get; set; }
    }
}