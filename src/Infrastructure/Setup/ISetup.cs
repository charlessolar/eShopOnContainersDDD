using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Setup
{
    public interface ISetup
    {
        Task<bool> Initialize();

        bool Done { get; }
    }
}
