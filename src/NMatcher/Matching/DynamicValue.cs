using NMatcher.Extensions;

namespace NMatcher.Matching
{
    public enum DynamicValueKind { Bool, Int, Double, String, Null, Guid, Array }

    internal static class DynamicValueKindExtractor
    {
        public static DynamicValueKind UnsafelyExtractKind(object value)
        {
            var rs = TryNull(value)
                .Or(TryBool(value))
                .Or(TryInt(value))
                .Or(TryDouble(value))
                .Or(TryGuid(value))
                .Or(TryString(value))
                .Or(TryArray(value));
            
            return rs.GetOrFail($"Unable to extract kind for unsupported type {value?.GetType().Name}.");
        }

        private static DynamicValueKind? TryBool(object value) =>
            value is bool ? DynamicValueKind.Bool : null;

        private static DynamicValueKind? TryString(object value) =>
            value is string ? DynamicValueKind.String : null;

        private static DynamicValueKind? TryInt(object value)
        {
            var res = value is short or ushort or int or uint or long or ulong;
            return res ? DynamicValueKind.Int : null;
        }
        
        private static DynamicValueKind? TryDouble(object value)
        {
            var res = value is float or double or decimal;
            return res ? DynamicValueKind.Double : null;
        }
        
        private static DynamicValueKind? TryNull(object value) =>
            value is null ? DynamicValueKind.Null : null;

        private static DynamicValueKind? TryArray(object value) =>
            value is not null && value.GetType().IsArray ? DynamicValueKind.Array : null;

        private static DynamicValueKind? TryGuid(object value) =>
            value is not null && System.Guid.TryParse(value.ToString(), out _) ? DynamicValueKind.Guid : null;
    }

    public record DynamicValue(object Value, DynamicValueKind Kind)
    {
        public static DynamicValue UnsafelyTryCreateFrom(object value)
        {
            var kind = DynamicValueKindExtractor.UnsafelyExtractKind(value);

            return new DynamicValue(value, kind);
        }
    }
}

