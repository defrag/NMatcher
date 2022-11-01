#nullable enable
using System;

namespace NMatcher.Extensions
{
    internal static class NullableExtensions
    {
        public static T1? Or<T1>(this T1? a, T1? b)
            where T1 : class
            => a ?? b;
        
        public static T1? Or<T1>(this T1? a, T1? b)
            where T1 : struct
            => a.HasValue ? a.Value : b;

        public static T2? Select<T1, T2>(this T1? v, Func<T1, T2> fn)
            where T1 : struct
            where T2 : class
            => v is not null ? (T2?)fn(v.Value) : null;
        
        public static T1 GetOrFail<T1>(this T1? a, string message)
        {
            if (a is null)
            {
                throw new ArgumentException(message);
            }

            return a;
        }
    }    
}
