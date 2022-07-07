using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.UnitOfWork.Query;
using Infrastructure;
using Infrastructure.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace eShop
{
    interface ICommitableCollection
    {
        Task Commit();
    }

    class CommitableCollection<T> : ICommitableCollection where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ILogger _logger;
        private readonly Dictionary<Id, T> _pendingSaves;
        private readonly Dictionary<Id, T> _pendingUpdates;
        private readonly List<Id> _pendingDeletes;

        public CommitableCollection(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>($"eshop;{typeof(T).FullName.ToLower()}", new MongoCollectionSettings { AssignIdOnInsert = false });
            _pendingSaves = new Dictionary<Id, T>();
            _pendingUpdates = new Dictionary<Id, T>();
            _pendingDeletes = new List<Id>();
            _logger = Log.Logger.For($"CommitCollection {typeof(T).FullName}");
        }

        public async Task<T> TryGet(Id id)
        {
            if (id == null)
                return null;

            _logger.DebugEvent("TryGet", "Retreiving document {Id}", id);

            FilterDefinition<T> filter;
            if (id.IsString())
            {
                filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>)"_id", (string)id);
            }
            else
            {
                filter = Builders<T>.Filter.Eq((FieldDefinition<T, Guid>)"_id", (Guid)id);
            }

            var result = await _collection.FindAsync(filter).ConfigureAwait(false);
            var document = await result.FirstOrDefaultAsync<T>().ConfigureAwait(false);
            if (document == null)
                _logger.WarnEvent("GetFailure", "Document {Id} was not found", id);
            return document;
        }

        public async Task<T> Get(Id id)
        {
            _logger.DebugEvent("Get", "Retreiving document {Id}", id);

            FilterDefinition<T> filter;
            if (id.IsString())
            {
                filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>)"_id", (string)id);
            }
            else
            {
                filter = Builders<T>.Filter.Eq((FieldDefinition<T, Guid>)"_id", (Guid)id);
            }

            var result = await _collection.FindAsync(filter).ConfigureAwait(false);
            var document = await result.FirstOrDefaultAsync<T>().ConfigureAwait(false);
            if (document == null)
            {
                _logger.WarnEvent("GetFailure", "Document {Id} was not found", id);
                throw new ArgumentException($"Document {id} was not found");
            }
            return document;
        }

        public void Add(Id id, T document)
        {
            _logger.DebugEvent("Add", "Queuing add document {Id}", id);
            _pendingSaves[id] = document;
        }

        public void Update(Id id, T document)
        {
            _logger.DebugEvent("Update", "Queuing update document {Id}", id);
            if (_pendingSaves.ContainsKey(id))
            {
                _pendingSaves[id] = document;
                return;
            }
            _pendingUpdates[id] = document;
        }

        public void Delete(Id id)
        {
            _logger.DebugEvent("Delete", "Queuing delete document {Id}", id);
            _pendingDeletes.Add(id);
            _pendingSaves.Remove(id);
            _pendingUpdates.Remove(id);
        }

        public async Task Commit()
        {
            _logger.DebugEvent("Commit", "Committing changes to collection {Type}", typeof(T).FullName);
            if (_pendingSaves.Any())
                await _collection.InsertManyAsync(_pendingSaves.Values).ConfigureAwait(false);
            _logger.DebugEvent("Commit", "Finished saves");
            if (_pendingUpdates.Any())
            {
                await _pendingUpdates.SelectAsync(doc =>
                {
                    FilterDefinition<T> filter;
                    if (doc.Key.IsString())
                    {
                        filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>)"_id", (string)doc.Key);
                    }
                    else
                    {
                        filter = Builders<T>.Filter.Eq((FieldDefinition<T, Guid>)"_id", (Guid)doc.Key);
                    }
                    return _collection.ReplaceOneAsync(filter, doc.Value);
                }).ConfigureAwait(false);
                _logger.DebugEvent("Commit", "Finished updates");
            }

            if (_pendingDeletes.Any())
            {
                await _pendingDeletes.SelectAsync(doc =>
                {
                    FilterDefinition<T> filter;
                    if (doc.IsString())
                    {
                        filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>)"_id", (string)doc);
                    }
                    else
                    {
                        filter = Builders<T>.Filter.Eq((FieldDefinition<T, Guid>)"_id", (Guid)doc);
                    }
                    return _collection.DeleteOneAsync(filter);
                }).ConfigureAwait(false);
                _logger.DebugEvent("Commit", "Finished deletes");
            }
        }
    }

    public class UnitOfWork : Aggregates.UnitOfWork.IApplicationUnitOfWork, Aggregates.UnitOfWork.IBaseUnitOfWork
    {
        public dynamic Bag { get; set; }

        private readonly IMongoDatabase _client;
        private readonly ILogger _logger;
        private readonly List<ICommitableCollection> _collections;


        public UnitOfWork(IMongoDatabase client)
        {
            _client = client;
            _collections = new List<ICommitableCollection>();
            _logger = Log.Logger.For<UnitOfWork>();
        }

        public Task Begin()
        {
            return Task.CompletedTask;
        }


        public async Task End(Exception ex = null)
        {
            if (ex != null) return;

            if (_collections.Any())
            {
                _logger.DebugEvent("Save", "Committing {Count} collections", _collections.Count);
                await _collections.SelectAsync(x => x.Commit()).ConfigureAwait(false);
            }
        }

        private CommitableCollection<T> GetOrAddCollection<T>() where T : class
        {
            var collection = _collections.SingleOrDefault(x => x is CommitableCollection<T>);
            if (collection == null)
            {
                collection = new CommitableCollection<T>(_client);
                _collections.Add(collection);
            }
            return collection as CommitableCollection<T>;
        }

        public Task Add<T>(Id id, T document) where T : class
        {
            var collection = GetOrAddCollection<T>();
            collection.Add(id, document);
            return Task.CompletedTask;
        }


        public Task Update<T>(Id id, T document) where T : class
        {
            var collection = GetOrAddCollection<T>();
            collection.Update(id, document);
            return Task.CompletedTask;
        }


        public Task<T> Get<T>(Id id) where T : class
        {
            var collection = GetOrAddCollection<T>();
            return collection.Get(id);
        }
        public Task<T> TryGet<T>(Id id) where T : class
        {
            var collection = GetOrAddCollection<T>();
            return collection.TryGet(id);
        }

        public Task Delete<T>(Id id) where T : class
        {
            var collection = GetOrAddCollection<T>();
            collection.Delete(id);
            return Task.CompletedTask;
        }

        public Task<IQueryResult<T>> Query<T>(IDefinition definition) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
