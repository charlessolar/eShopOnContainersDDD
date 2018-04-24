using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class QueryResult<T> : Infrastructure.IQueryResult<T> where T : class
    {
        public T[] Records { get; set; }
        public long Total { get; set; }
        public long ElapsedMs { get; set; }
    }

    public class QueryBuilder
    {
        internal class FieldQueryDefinition : Infrastructure.FieldQueryDefinition
        {
            public string Field { get; set; }
            public string Value { get; set; }
            public Operation Op { get; set; }
        }

        internal class QueryDefinition : Infrastructure.QueryDefinition
        {
            public List<Tuple<Group, Infrastructure.FieldQueryDefinition[]>> FieldDefinitions { get; set; }
            public long Skip { get; set; }
            public long Take { get; set; }
        }

        public class GroupedBuilder
        {
            internal readonly Group _group;
            internal readonly List<Infrastructure.FieldQueryDefinition> _fields;
            public GroupedBuilder(Group group)
            {
                _group = group;
                _fields = new List<Infrastructure.FieldQueryDefinition>();
            }

            public GroupedBuilder Add(string field, string value, Operation op)
            {
                _fields.Add(new FieldQueryDefinition { Field = field, Value = value, Op = op });
                return this;
            }
        }


        private readonly List<GroupedBuilder> _groups;
        public QueryBuilder()
        {
            _groups = new List<GroupedBuilder>();
        }

        public QueryBuilder Add(string field, string value, Operation op)
        {
            _groups.Add(new GroupedBuilder(Group.ALL).Add(field, value, op));
            return this;
        }

        public GroupedBuilder Grouped(Group group)
        {
            return new GroupedBuilder(group);
        }

        public Infrastructure.QueryDefinition Build()
        {
            var definition = new QueryDefinition();
            foreach (var group in _groups)
            {
                definition.FieldDefinitions.Add(Tuple.Create(group._group, group._fields.ToArray()));
            }
            return definition;
        }
    }
}
