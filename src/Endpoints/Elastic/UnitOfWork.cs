using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Infrastructure;
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

                // refresh all indicies which were "inserted" into so we can GET by Id immediately
                var indices = _pendingDocs.Values.Where(x => x.Operation == "index");
                if (indices.Any())
                    await _client.RefreshAsync(indices.Select(x => x.Index).ToArray()).ConfigureAwait(false);
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

        private QueryContainer addOperation<T>(QueryContainerDescriptor<T> descriptor,
            FieldQueryDefinition definition) where T : class
        {
            switch (definition.Op)
            {
                case Operation.EQUAL:
                    return descriptor.Term(term => term.Field(definition.Field).Value(definition.Value));
                case Operation.CONTAINS:
                    return descriptor.Term(term => term.Field(definition.Field).Value(definition.Value));
                case Operation.GREATER_THAN:
                    return descriptor.Range(number => number.Field(definition.Field).GreaterThan(double.Parse(definition.Value)));
                case Operation.GREATER_THAN_OR_EQUAL:
                    return descriptor.Range(number => number.Field(definition.Field).GreaterThanOrEquals(double.Parse(definition.Value)));
                case Operation.LESS_THAN:
                    return descriptor.Range(number => number.Field(definition.Field).LessThan(double.Parse(definition.Value)));
                case Operation.LESS_THAN_OR_EQUAL:
                    return descriptor.Range(number => number.Field(definition.Field).LessThanOrEquals(double.Parse(definition.Value)));
                case Operation.NOT_EQUAL:
                    return !descriptor.Term(term => term.Field(definition.Field).Value(definition.Value));

            }

            throw new ArgumentException($"operation {definition.Op} is not supported");
        }

        public async Task<IQueryResult<T>> Query<T>(QueryDefinition definition) where T : class
        {
            Func<SearchDescriptor<T>, ISearchRequest> searchSelector = x =>
            {
                x = x.Query(q =>
                {
                    foreach (var group in definition.FieldDefinitions)
                    {
                        switch (group.Item1)
                        {
                            case Group.ALL:
                                {
                                    QueryContainer final = new MatchAllQuery();
                                    foreach (var match in group.Item2)
                                    {
                                        final = final && addOperation(q, match);
                                    }

                                    q.Bool(b => b.Must(final));
                                    break;
                                }
                            case Group.NOT:
                                {
                                    QueryContainer final = new MatchAllQuery();
                                    foreach (var match in group.Item2)
                                    {
                                        final = final && addOperation(q, match);
                                    }

                                    q.Bool(b => b.Must(!final));
                                    break;
                                }
                            case Group.ANY:
                                {
                                    QueryContainer final = new MatchAllQuery();
                                    foreach (var match in group.Item2)
                                    {
                                        final = final || addOperation(q, match);
                                    }

                                    q.Bool(b => b.Must(final));
                                    break;
                                }
                        }
                    }

                    return q;
                });

                return x;
            };


            var documents = new List<T>();
            var elapsedMs = 0L;
            ISearchResponse<T> searchResult = await _client.SearchAsync<T>(x =>
            {
                x = x.Index(typeof(T).FullName.ToLower());
                x = x.SearchType(SearchType.QueryThenFetch);
                x = x.Scroll("4s");

                return searchSelector(x);
            }).ConfigureAwait(false);

            if (!searchResult.IsValid)
            {
                _logger.WarnEvent("Invalid", "Elastic request: {Request}", searchResult.DebugInformation);
                throw new StorageException($"Invalid elastic request: {searchResult.DebugInformation}");
            }

            do
            {
                documents.AddRange(searchResult.Documents);
                elapsedMs += searchResult.Took;

                searchResult = await _client.ScrollAsync<T>("4s", searchResult.ScrollId).ConfigureAwait(false);

                if (!searchResult.IsValid)
                    throw new StorageException($"Invalid elastic request: {searchResult.DebugInformation}");
            } while (searchResult.Documents.Any());

            return new QueryResult<T>
            {
                Records = documents.ToArray(),
                Total = searchResult.Total,
                ElapsedMs = elapsedMs
            };
        }
    }
}
