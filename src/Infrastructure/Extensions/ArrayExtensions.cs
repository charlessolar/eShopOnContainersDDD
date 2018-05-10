using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class ArrayExtensions
    {
        public static string[] TryAdd(this string[] source, string element)
        {
            if (source == null)
                source = new string[] { };

            if (source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret.ToArray();
        }
        public static T[] TryAdd<T>(this T[] source, T element) where T : struct
        {
            if (source == null)
                source = new T[] { };

            if (source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret.ToArray();
        }
        public static T[] TryAdd<T, TK>(this T[] source, T element, Func<T, TK> selector) where T : class
        {
            if (source == null)
                source = new T[] { };

            if (source.Any(x => selector.Invoke(x).Equals(selector.Invoke(element))))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret.ToArray();
        }
        public static string[] TryRemove(this string[] source, string element)
        {
            if (source == null)
                source = new string[] { };

            if (!source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Remove(element);
            return ret.ToArray();
        }
        public static T[] TryRemove<T>(this T[] source, T element) where T : struct
        {
            if (source == null)
                source = new T[] { };

            if (!source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Remove(element);
            return ret.ToArray();
        }
        public static T[] TryRemove<T, TK>(this T[] source, TK key, Func<T, TK> selector)
        {
            if (source == null)
                source = new T[] { };

            var idx = source.SingleOrDefault(x => selector.Invoke(x).Equals(key));
            if (idx == null)
                return source;

            var ret = source.ToList();
            ret.Remove(idx);
            return ret.ToArray();
        }
        public static T[] TryRemove<T, TU>(this T[] source, T element, Func<T, TU> selector) where T : class
        {
            if (source == null)
                source = new T[] { };

            var idx = source.SingleOrDefault(x => selector.Invoke(x).Equals(selector.Invoke(element)));
            if (idx == null)
                return source;

            var ret = source.ToList();
            ret.Remove(idx);
            return ret.ToArray();
        }
    }
}
