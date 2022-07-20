using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public interface IGrouped
    {
        string Group { get; set; }
        IFieldDefinition[] Definitions { get; set; }
    }
}