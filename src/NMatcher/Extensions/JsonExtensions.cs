using System.Linq;
using System.Text.Json;

namespace NMatcher.Extensions
{
    internal static class JsonExtensions
    {
        public static object ParseValue(this JsonElement e)
        {
            return e.ValueKind switch
            {
                JsonValueKind.String => e.GetString()!,
                JsonValueKind.Number => e
                    .MaybeGetInt().Select(v => (object)v)
                    .Or(e.MaybeGetDouble().Select(v => (object) v))
                    .GetOrFail(),
                JsonValueKind.False => false,
                JsonValueKind.True => true,
                JsonValueKind.Array => e.EnumerateArray().ToArray().Select(e2 => e2.ParseValue()).ToArray(),
                JsonValueKind.Object => e.EnumerateObject().ToArray().Select(e2 => e2.Value.ParseValue()).ToArray(),
                JsonValueKind.Null => null,
            };
        }
        
        public static int? MaybeGetInt(this JsonElement e)
        {
            if (e.TryGetInt32(out var v))
            {
                return v;
            }

            return null;
        }
        
        public static double? MaybeGetDouble(this JsonElement e)
        {
            if (e.TryGetDouble(out var v))
            {
                return v;
            }

            return null;
        }
    }
}