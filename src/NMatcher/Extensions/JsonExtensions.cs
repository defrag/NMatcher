#nullable enable
using System;
using System.Linq;
using System.Text.Json;
using NMatcher.Matching;
using Guid = System.Guid;

namespace NMatcher.Extensions
{
    internal static class JsonExtensions
    {
        public static DynamicValue? ParseValue(this JsonElement e)
        {
            return e.ValueKind switch
            {
                JsonValueKind.String => e.MaybeGetGuid().Select(v => DynamicValue.Create(v)).Or(DynamicValue.Create(e.GetString()!)),
                JsonValueKind.Number => e
                    .MaybeGetInt().Select(v => DynamicValue.Create(v))
                    .Or(e.MaybeGetDouble().Select(v => DynamicValue.Create(v)))
                    .GetOrFail($"Unable to get the number value from {e.ValueKind}."),
                JsonValueKind.False => DynamicValue.Create(false),
                JsonValueKind.True => DynamicValue.Create(true),
                JsonValueKind.Array => DynamicValue.Create(e.EnumerateArray().ToArray().Select(e2 => e2.ParseValue()).ToArray()),
                JsonValueKind.Object => DynamicValue.Create(e.EnumerateObject().ToArray().Select(e2 => e2.Value.ParseValue()).ToArray()),
                JsonValueKind.Null => DynamicValue.Create(null),
                JsonValueKind.Undefined => null,
                _ => throw new MatchingException($"Parsing value of kind {e.ValueKind} is not supported.")
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
        
        public static Guid? MaybeGetGuid(this JsonElement e)
        {
            if (e.TryGetGuid(out var v))
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