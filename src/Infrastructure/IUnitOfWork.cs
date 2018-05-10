using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public enum Operation
    {
        EQUAL,
        NOT_EQUAL,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        CONTAINS,
    }

    public enum Group
    {
        ALL,
        ANY,
        NOT
    }
    public interface QueryDefinition
    {
        List<Tuple<Group, FieldQueryDefinition[]>> FieldDefinitions { get; set; }

        long Skip { get; set; }
        long Take { get; set; }
    }
    public interface FieldQueryDefinition
    {
        string Field { get; set; }

        string Value { get; set; }

        Operation Op { get; set; }
    }

    public interface IQueryResult<T> where T : class
    {
        T[] Records { get; set; }
        long Total { get; set; }
        long ElapsedMs { get; set; }
    }

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

        Task<IQueryResult<T>> Query<T>(QueryDefinition query) where T : class;
    }
}
