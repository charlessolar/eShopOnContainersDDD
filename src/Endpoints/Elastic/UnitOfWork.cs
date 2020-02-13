using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.UnitOfWork.Query;
using Elasticsearch.Net;
using Infrastructure;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Serilog;

namespace eShop
{
    public class UnitOfWork : Infrastructure.IUnitOfWork, Aggregates.UnitOfWork.IUnitOfWork
    {
        public static Nest.Id ToId(Id id)
        {
            if (id.IsLong())
                return new Nest.Id((long)id);

            return new Nest.Id(id.ToString());
        }

        public dynamic Bag { get; set; }

        private readonly Nest.IElasticClient _client;
        private readonly ILogger _logger;

        private readonly Dictionary<string, Nest.IBulkOperation> _pendingDocs;
        private readonly Dictionary<string, long> _versions;

        public UnitOfWork(Nest.IElasticClient client)
        {
            _client = client;
            _logger = Log.Logger.For<UnitOfWork>();
            _pendingDocs = new Dictionary<string, Nest.IBulkOperation>();
            _versions = new Dictionary<string, long>();
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

            if (_pendingDocs.Any())
            {
                _logger.DebugEvent("Save", "Committing {Count} operations", _pendingDocs.Count);
                var pending = new Nest.BulkDescriptor();
                foreach (var op in _pendingDocs.Values)
                    pending.AddOperation(op);

                // Index the pending docs
                //var response = await _client.BulkAsync(_pendingDocs.Consistency(Consistency.Quorum)).ConfigureAwait(false);
                var response = await _client.BulkAsync(pending).ConfigureAwait(false);
                if (response.Errors)
                {
                    foreach (var item in response.Items.Select(x => $"{x.Type}, {x.Id}").Except(response.ItemsWithErrors.Select(x => $"{x.Type}, {x.Id}")))
                        Bag.Saved.Add(item);

                    throw new StorageException(response.DebugInformation);
                }

                // refresh all indicies which were "inserted" into so we can GET by Id immediately
                // not needed with latest ES versions?
                //var indices = _pendingDocs.Values.Where(x => x.Operation == "index");
                //if (indices.Any())
                //    await _client.RefreshAsync(indices.Select(x => x.Index).ToArray()).ConfigureAwait(false);
            }
        }

        public Task Add<T>(Id id, T document) where T : class
        {
            if (Bag.Saved.Contains($"{typeof(T).FullName}, {id}"))
                return Task.CompletedTask;

            _logger.DebugEvent("Index", "Object {Object} Document {Id}", typeof(T).FullName, id);
            _pendingDocs[$"{typeof(T).FullName}, {id}"] = new Nest.BulkIndexDescriptor<T>().Index(typeof(T).FullName.ToLower()).Id(ToId(id)).Document(document);

            return Task.CompletedTask;
        }

        public Task Update<T>(Id id, T document) where T : class
        {
            if (Bag.Saved.Contains($"{typeof(T).FullName}, {id}"))
                return Task.CompletedTask;

            var descriptor = new Nest.BulkUpdateDescriptor<T, object>().Index(typeof(T).FullName.ToLower()).Id(ToId(id))
                .Doc(document);
            if (_versions.ContainsKey($"{typeof(T).FullName}, {id}"))
                descriptor.Version(_versions[$"{typeof(T).FullName}, {id}"]);
            else
                _logger.WarnEvent("NoVersion",
                    "Update document {Document} id {Id} lacks version information - try to GET it first");

            _logger.DebugEvent("Update", "Object {Object} Document {Id}", typeof(T).FullName, id);
            _pendingDocs[$"{typeof(T).FullName}, {id}"] = descriptor;

            return Task.CompletedTask;
        }
        public async Task<T> Get<T>(Id id) where T : class
        {
            _logger.DebugEvent("Get", "Object {Object} Document {Id}", typeof(T).FullName, id);

            var response = await _client.GetAsync<T>(new Nest.GetRequest<T>(typeof(T).FullName.ToLower(), ToId(id))).ConfigureAwait(false);
            if (!response.Found)
            {
                _logger.WarnEvent("GetFailure", "Object {Object} Document {Id} not found!", typeof(T).FullName, id);
                throw new ArgumentException($"Document {id} was not found");
            }

            _versions[$"{typeof(T).FullName}, {id}"] = response.Version;

            return response.Source;
        }
        public async Task<T> TryGet<T>(Id id) where T : class
        {
            _logger.DebugEvent("TryGet", "Object {Object} Document {Id}", typeof(T).FullName, id);
            if (id == null)
                return null;

            var response = await _client.GetAsync<T>(new Nest.GetRequest<T>(typeof(T).FullName.ToLower(), ToId(id))).ConfigureAwait(false);
            if (!response.Found)
            {
                _logger.WarnEvent("GetFailure", "Object {Object} Document {Id} not found!", typeof(T).FullName, id);
                return null;
            }

            _versions[$"{typeof(T).FullName}, {id}"] = response.Version;

            return response.Source;
        }

        public Task Delete<T>(Id id) where T : class
        {
            if (Bag.Saved.Contains($"{typeof(T).FullName}, {id}"))
                return Task.CompletedTask;

            _logger.DebugEvent("Delete", "Object {Object} Document {Id}", typeof(T).FullName, id);
            var operation = new Nest.BulkDeleteOperation<T>(ToId(id));
            operation.Index = typeof(T).FullName.ToLower();
            _pendingDocs[$"{typeof(T).FullName}, {id}"] = operation;

            return Task.CompletedTask;
        }

        private Nest.QueryContainer addOperation<T>(Nest.QueryContainerDescriptor<T> descriptor,
            IFieldDefinition definition) where T : class
        {
            var operation = Operation.FromValue(definition.Op);

            if (operation == Operation.Equal)
                return descriptor.Term(term => term.Field(definition.Field).Value(definition.Value).Boost(definition.Boost));
            if (operation == Operation.Contains)
                return descriptor.Match(match => match.Field(definition.Field).Query(definition.Value).Boost(definition.Boost));
            if (operation == Operation.GreaterThan)
                return descriptor.Range(number => number.Field(definition.Field).GreaterThan(double.Parse(definition.Value)).Boost(definition.Boost));
            if (operation == Operation.GreaterThanOrEqual)
                return descriptor.Range(number => number.Field(definition.Field).GreaterThanOrEquals(double.Parse(definition.Value)).Boost(definition.Boost));
            if (operation == Operation.LessThan)
                return descriptor.Range(number => number.Field(definition.Field).LessThan(double.Parse(definition.Value)).Boost(definition.Boost));
            if (operation == Operation.LessThanOrEqual)
                return descriptor.Range(number => number.Field(definition.Field).LessThanOrEquals(double.Parse(definition.Value)).Boost(definition.Boost));
            if (operation == Operation.NotEqual)
                return !descriptor.Term(term => term.Field(definition.Field).Value(definition.Value).Boost(definition.Boost));
            if (operation == Operation.Autocomplete)
                return descriptor.MultiMatch(match => match.Fields(f => f
                    .Field($"{definition.Field}.default", 10)
                    .Field($"{definition.Field}.stemmed", 2)
                    .Field($"{definition.Field}.shingles", 2)
                    .Field($"{definition.Field}.ngram"))
                    .Query(definition.Value)
                    .Operator(Nest.Operator.And)
                    );

            throw new ArgumentException($"operation {definition.Op} is not supported");
        }

        public async Task<IQueryResult<T>> Query<T>(IDefinition definition) where T : class
        {
            Func<Nest.SearchDescriptor<T>, Nest.ISearchRequest> searchSelector = x =>
            {
                x = x.Query(q =>
                {
                    foreach (var group in definition.Operations)
                    {
                        var option = Group.FromValue(group.Group);
                        if (option == Group.All)
                        {
                            Nest.QueryContainer final = new Nest.MatchAllQuery();
                            foreach (var match in group.Definitions)
                            {
                                final = final && addOperation(q, match);
                            }

                            q.Bool(b => b.Must(final));
                        }
                        else if (option == Group.Not)
                        {
                            Nest.QueryContainer final = new Nest.MatchAllQuery();
                            foreach (var match in group.Definitions)
                            {
                                final = final && addOperation(q, match);
                            }

                            q.Bool(b => b.Must(!final));
                        }
                        else if (option == Group.Any)
                        {
                            Nest.QueryContainer final = new Nest.MatchAllQuery();
                            foreach (var match in group.Definitions)
                            {
                                final = final || addOperation(q, match);
                            }

                            q.Bool(b => b.Must(final));
                        }
                    }

                    return q;
                });

                return x;
            };


            var documents = new List<T>();
            var elapsedMs = 0L;
            Nest.ISearchResponse<T> searchResult = await _client.SearchAsync<T>(x =>
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
