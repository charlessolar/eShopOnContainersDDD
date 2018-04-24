using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    class CommitableCollection<T> : ICommitableCollection
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ILogger _logger;
        private readonly Dictionary<string, T> _retreived;
        private readonly Dictionary<string, T> _pendingSaves;
        private readonly Dictionary<string, T> _pendingUpdates;
        private readonly List<string> _pendingDeletes;

        public CommitableCollection(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>("", new MongoCollectionSettings {AssignIdOnInsert = false});
            _retreived = new Dictionary<string, T>();
            _pendingSaves = new Dictionary<string, T>();
            _pendingUpdates = new Dictionary<string, T>();
            _pendingDeletes = new List<string>();
            _logger = Log.Logger.With<CommitableCollection<T>>();
        }

        public async Task<T> Get(string id)
        {
            _logger.DebugEvent("Get", "Retreiving document {Id}", id);
            if (_retreived.ContainsKey(id))
                return _retreived[id];

            var filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>) "Id", id);
            var result = await _collection.FindAsync(filter).ConfigureAwait(false);
            var document = await result.FirstAsync<T>().ConfigureAwait(false);
            _retreived[id] = document;
            return document;
        }

        public void Add(string id, T document)
        {
            _logger.DebugEvent("Add", "Queuing add document {Id}", id);
            _pendingSaves[id] = document;
        }

        public void Update(string id, T document)
        {
            _logger.DebugEvent("Update", "Queuing update document {Id}", id);
            if (_pendingSaves.ContainsKey(id))
            {
                _pendingSaves[id] = document;
                return;
            }
            _pendingUpdates[id] = document;
        }

        public void Delete(string id)
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
            if(_pendingUpdates.Any())
            {
                await _pendingUpdates.SelectAsync(doc =>
                {
                    var filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>) "Id", doc.Key);
                    return _collection.ReplaceOneAsync(filter, doc.Value);
                }).ConfigureAwait(false);
            }

            if (_pendingDeletes.Any())
            {
                await _pendingDeletes.SelectAsync(doc =>
                {
                    var filter = Builders<T>.Filter.Eq((FieldDefinition<T, string>) "Id", doc);
                    return _collection.DeleteOneAsync(filter);
                }).ConfigureAwait(false);
            }
        }
    }

    public class UnitOfWork : Infrastructure.IUnitOfWork
    {
        public dynamic Bag { get; set; }

        private readonly IMongoDatabase _client;
        private readonly ILogger _logger;
        private readonly List<ICommitableCollection> _collections;


        public UnitOfWork(IMongoDatabase client)
        {
            _client = client;
            _collections = new List<ICommitableCollection>();
            _logger = Log.Logger.With<UnitOfWork>();
        }

        public Task Begin()
        {
            if (!Aggregates.Dynamic.ContainsProperty(Bag, "Saved"))
                Bag.Saved = new HashSet<string>();
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

        private CommitableCollection<T> GetOrAddCollection<T>()
        {
            var collection = _collections.SingleOrDefault(x => x is CommitableCollection<T>);
            if (collection == null)
            {
                collection = new CommitableCollection<T>(_client);
                _collections.Add(collection);
            }
            return collection as CommitableCollection<T>;
        }

        public Task Add<T>(string id, T document) where T : class
        {
            var collection = GetOrAddCollection<T>();
            collection.Add(id, document);
            return Task.CompletedTask;
        }

        public Task Add<T>(Guid id, T document) where T : class
        {
            return Add(id.ToString(), document);
        }

        public Task Update<T>(string id, T document) where T : class
        {
            var collection = GetOrAddCollection<T>();
            collection.Update(id, document);
            return Task.CompletedTask;
        }

        public Task Update<T>(Guid id, T document) where T : class
        {
            return Update(id.ToString(), document);
        }

        public Task<T> Get<T>(string id) where T : class
        {
            var collection = GetOrAddCollection<T>();
            return collection.Get(id);
        }

        public Task<T> Get<T>(Guid id) where T : class
        {
            return Get<T>(id.ToString());
        }

        public Task Delete<T>(string id) where T : class
        {
            var collection = GetOrAddCollection<T>();
            collection.Delete(id);
            return Task.CompletedTask;
        }

        public Task Delete<T>(Guid id) where T : class
        {
            return Delete<T>(id.ToString());
        }

        public Task<IQueryResult<T>> Query<T>(QueryDefinition definition) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
