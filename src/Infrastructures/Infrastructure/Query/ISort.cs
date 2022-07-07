using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public interface ISort
    {
        string Field { get; set; }
        string Dir { get; set; }
    }
}