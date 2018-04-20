using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUnitOfWork : Aggregates.IUnitOfWork
    {
        Task Add<T>(T document);
        Task Add<T>(string id, T document);
        Task Add<T>(Guid id, T document);
        Task Update<T>(T document);
        Task Update<T>(string id, T document);
        Task Update<T>(Guid id, T document);

        Task<T> Get<T>(string id);
        Task<T> Get<T>(Guid id);

        Task Delete<T>(string id);
        Task Delete<T>(Guid id);
    }
}
