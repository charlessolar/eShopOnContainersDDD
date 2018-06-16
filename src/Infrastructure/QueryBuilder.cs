using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aggregates.UnitOfWork.Query;

namespace Infrastructure
{
    public class QueryResult<T> : IQueryResult<T> where T : class
    {
        public T[] Records { get; set; }
        public long Total { get; set; }
        public long ElapsedMs { get; set; }
    }

    public class QueryBuilder
    {
        internal class FieldQueryDefinition : IFieldDefinition
        {
            public string Field { get; set; }
            public string Value { get; set; }
            public string Op { get; set; }
            public double? Boost { get; set; }
        }

        internal class QueryDefinition : IDefinition
        {
            public IGrouped[] Operations { get; set; }
            public long? Skip { get; set; }
            public long? Take { get; set; }
            public ISort[] Sort { get; set; }
        }
        internal class GroupedInfo : IGrouped
        {
            public string Group { get; set; }
            public IFieldDefinition[] Definitions { get; set; }
        }

        public class GroupedBuilder
        {
            internal GroupedInfo Grouped => new GroupedInfo { Group = _group.Value, Definitions = _fields.ToArray() };

            internal readonly Group _group;
            internal readonly List<FieldQueryDefinition> _fields;

            public GroupedBuilder(Group group)
            {
                _group = group;
                _fields = new List<FieldQueryDefinition>();
            }

            public GroupedBuilder Add(string field, string value, Operation op, double? boost = null)
            {
                _fields.Add(new FieldQueryDefinition { Field = field, Value = value, Op = op.Value, Boost = boost });
                return this;
            }
        }


        private readonly List<GroupedBuilder> _groups;
        public QueryBuilder()
        {
            _groups = new List<GroupedBuilder>();
        }

        public QueryBuilder Add(string field, string value, Operation op, double? boost = null)
        {
            _groups.Add(new GroupedBuilder(Group.All).Add(field, value, op, boost));
            return this;
        }

        public GroupedBuilder Grouped(Group group)
        {
            return new GroupedBuilder(group);
        }

        public IDefinition Build()
        {
            var definitions = new List<GroupedInfo>();
            foreach (var group in _groups)
            {
                definitions.Add(group.Grouped);
            }

            return new QueryDefinition
            {
                Operations = definitions.ToArray()
            };
        }
    }
}
