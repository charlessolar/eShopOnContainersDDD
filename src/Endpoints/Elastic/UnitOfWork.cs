using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Nest;
using Serilog;

namespace eShop
{
    public class UnitOfWork : Infrastructure.IUnitOfWork
    {
        public dynamic Bag { get; set; }

        private readonly IElasticClient _client;
        private readonly ILogger _logger;

        private readonly Dictionary<Id, IBulkOperation> _pendingDocs;

        public UnitOfWork(IElasticClient client)
        {
            _client = client;
            _logger = Log.Logger.With<UnitOfWork>();
            _pendingDocs = new Dictionary<Id, IBulkOperation>();
        }

        public Task Begin()
        {
            if (!Aggregates.Dynamic.ContainsProperty(Bag, "Saved"))
                Bag.Saved = new HashSet<Id>();
            return Task.CompletedTask;
        }

        public async Task End(Exception ex = null)
        {
            if (ex != null) return;

            if (_pendingDocs.Any())
            {
                _logger.DebugEvent("Save", "Committing {Count} operations", _pendingDocs.Count);
                var pending = new BulkDescriptor();
                foreach (var op in _pendingDocs.Values)
                    pending.AddOperation(op);

                // Index the pending docs
                //var response = await _client.BulkAsync(_pendingDocs.Consistency(Consistency.Quorum)).ConfigureAwait(false);
                IBulkResponse response;
                response = await _client.BulkAsync(pending).ConfigureAwait(false);
                if (response.Errors)
                {
                    foreach (var item in response.Items.Select(x => x.Id).Except(response.ItemsWithErrors.Select(x => x.Id)))
                        Bag.Saved.Add(item);

                    throw new StorageException(response.DebugInformation);
                }
            }
        }
        
        public Task Add<T>(string id, T document) where T : class
        {
            if (Bag.Saved.Contains(id))
                return Task.CompletedTask;

            _logger.DebugEvent("Index", "Object {Object} Document {Id}", typeof(T).FullName, id);
            _pendingDocs[id] = new BulkIndexDescriptor<T>().Index(typeof(T).FullName.ToLower()).Id(id).Document(document);

            return Task.CompletedTask;
        }

        public Task Add<T>(Guid id, T document) where T : class
        {
            return Add<T>(id.ToString(), document);
        }
        
        public Task Update<T>(string id, T document) where T : class
        {
            if (Bag.Saved.Contains(id))
                return Task.CompletedTask;

            _logger.DebugEvent("Update", "Object {Object} Document {Id}", typeof(T).FullName, id);
            _pendingDocs[id] = new BulkUpdateDescriptor<T, object>().Index(typeof(T).FullName.ToLower()).Id(id).Doc(document);

            return Task.CompletedTask;
        }

        public Task Update<T>(Guid id, T document) where T : class
        {
            return Update<T>(id.ToString(), document);
        }

        public async Task<T> Get<T>(string id) where T : class
        {
            _logger.DebugEvent("Get", "Object {Object} Document {Id}", typeof(T).FullName, id);

            var response = await _client.GetAsync<T>(new GetRequest<T>(typeof(T).FullName.ToLower(), typeof(T).FullName, id)).ConfigureAwait(false);
            if (!response.Found) return null;

            return response.Source;
        }

        public Task<T> Get<T>(Guid id) where T : class
        {
            return Get<T>(id.ToString());
        }

        public Task Delete<T>(string id) where T : class
        {
            if (Bag.Saved.Contains(id))
                return Task.CompletedTask;

            _logger.DebugEvent("Delete", "Object {Object} Document {Id}", typeof(T).FullName, id);
            var operation = new BulkDeleteOperation<T>(id);
            operation.Index = typeof(T).FullName.ToLower();
            _pendingDocs[id] = operation;

            return Task.CompletedTask;
        }

        public Task Delete<T>(Guid id) where T : class
        {
            return Delete<T>(id.ToString());
        }
    }
}
