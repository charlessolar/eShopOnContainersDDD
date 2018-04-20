using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUnitOfWork : Aggregates.IUnitOfWork
    {
        Task Add<T>(string id, T document) where T : class;
        Task Add<T>(Guid id, T document) where T : class;
        Task Update<T>(string id, T document) where T : class;
        Task Update<T>(Guid id, T document) where T : class;

        Task<T> Get<T>(string id) where T : class;
        Task<T> Get<T>(Guid id) where T : class;

        Task Delete<T>(string id) where T : class;
        Task Delete<T>(Guid id) where T : class;
    }
}
