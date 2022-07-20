using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IMongoDb : Aggregates.UnitOfWork.IApplicationUnitOfWork
    {
    }
}
