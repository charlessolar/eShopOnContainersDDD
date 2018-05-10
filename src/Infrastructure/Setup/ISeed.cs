using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Setup
{
    public interface ISeed
    {
        Task<bool> Seed();
        
        bool Started { get; }
    }
}
