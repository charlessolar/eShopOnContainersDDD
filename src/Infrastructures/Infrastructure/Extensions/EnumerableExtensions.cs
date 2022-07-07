using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> TryAdd(this IEnumerable<string> source, string element)
        {
            if (source == null)
                source = new string[] { };

            if (source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret;
        }
        public static IEnumerable<T> TryAdd<T>(this IEnumerable<T> source, T element) where T : struct
        {
            if (source == null)
                source = new T[] { };

            if (source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret;
        }
        public static IEnumerable<T> TryAdd<T, TK>(this IEnumerable<T> source, T element, Func<T, TK> selector) where T : class
        {
            if (source == null)
                source = new T[] { };

            if (source.Any(x => selector.Invoke(x).Equals(selector.Invoke(element))))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret;
        }
        public static IEnumerable<string> TryRemove(this IEnumerable<string> source, string element)
        {
            if (source == null)
                source = new string[] { };

            if (!source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Remove(element);
            return ret;
        }
        public static IEnumerable<T> TryRemove<T>(this IEnumerable<T> source, T element) where T : struct
        {
            if (source == null)
                source = new T[] { };

            if (!source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Remove(element);
            return ret;
        }
        public static IEnumerable<T> TryRemove<T, TK>(this IEnumerable<T> source, TK key, Func<T, TK> selector)
        {
            if (source == null)
                source = new T[] { };

            var idx = source.SingleOrDefault(x => selector.Invoke(x).Equals(key));
            if (idx == null)
                return source;

            var ret = source.ToList();
            ret.Remove(idx);
            return ret;
        }
        public static IEnumerable<T> TryRemove<T, TU>(this IEnumerable<T> source, T element, Func<T, TU> selector) where T : class
        {
            if (source == null)
                source = new T[] { };

            var idx = source.SingleOrDefault(x => selector.Invoke(x).Equals(selector.Invoke(element)));
            if (idx == null)
                return source;

            var ret = source.ToList();
            ret.Remove(idx);
            return ret;
        }
    }
}
