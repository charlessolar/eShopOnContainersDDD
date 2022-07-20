using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IElastic : Aggregates.UnitOfWork.IApplicationUnitOfWork
    {
    }
}
