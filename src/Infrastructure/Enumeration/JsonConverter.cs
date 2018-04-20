using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Infrastructure.Enumeration
{
    public class JsonConverter : Newtonsoft.Json.JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object>> EnumerationsFromValue = new ConcurrentDictionary<Type, Func<object, object>>();
        private static readonly ConcurrentDictionary<Type, Func<object, object>> ValueFromEnumeration = new ConcurrentDictionary<Type, Func<object, object>>();

        public override bool CanConvert(Type objectType) => objectType.BaseType != null && objectType.BaseType.IsGenericType && objectType.BaseType.GetGenericTypeDefinition() == typeof(Infrastructure.Enumeration.Enumeration<,>);

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
                throw new Exception(string.Format("This converter is not for {0}.", objectType));

            var factory = EnumerationsFromValue.GetOrAdd(objectType, (_) => MakeFuncEnumerationDelegate(objectType));

            return factory(reader.Value);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var enumerationType = value.GetType();


            var factory = ValueFromEnumeration.GetOrAdd(enumerationType, (_) => MakeFuncValueDelegate(enumerationType));

            writer.WriteValue(factory(value));
        }

        private Func<object, object> MakeFuncEnumerationDelegate(Type enumerationType)
        {
            var valueType = enumerationType.BaseType.GetGenericArguments()[1];

            var method = enumerationType.GetMethod("FromValue", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            var param1 = Expression.Parameter(typeof(object));

            var castTarget = Expression.Convert(param1, valueType);

            Expression body = Expression.Call(method, castTarget);

            return Expression.Lambda<Func<object, object>>(body, param1).Compile();
        }
        private Func<object, object> MakeFuncValueDelegate(Type enumerationType)
        {
            var valueType = enumerationType.BaseType.GetGenericArguments()[1];

            var method = enumerationType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            var target = Expression.Parameter(typeof(object));

            var castTarget = Expression.Convert(target, enumerationType);

            Expression body = Expression.Call(castTarget, method.GetMethod);

            return Expression.Lambda<Func<object, object>>(body, target).Compile();
        }
    }
}
