using System;
using System.Globalization;
using System.Linq;
using NMatcher.Extensions;
using NMatcher.Matching;

namespace NMatcher.Matching2
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
            var res = value is short || 
                      value is ushort ||
                      value is int ||
                      value is uint ||
                      value is long ||
                      value is ulong;
            return res ? DynamicValueKind.Int : null;
        }
        
        private static DynamicValueKind? TryDouble(object value)
        {
            var res = value is float ||
                       value is double ||
                       value is decimal;
            return res ? DynamicValueKind.Double : null;
        }
        
        private static DynamicValueKind? TryNull(object value) =>
            value is null ? DynamicValueKind.Null : null;

        private static DynamicValueKind? TryArray(object value) =>
            value.GetType().IsArray ? DynamicValueKind.Array : null;

        private static DynamicValueKind? TryGuid(object value) =>
            System.Guid.TryParse(value.ToString(), out _) ? DynamicValueKind.Guid : null;
    }

    public record DynamicValue(object Value, DynamicValueKind Kind)
    {
        public T UnsafelyParseValue<T>() => (T)Value;

        public static DynamicValue UnsafelyTryCreateFrom(object value)
        {
            var kind = DynamicValueKindExtractor.UnsafelyExtractKind(value);

            return new DynamicValue(value, kind);
        }
    }
    
    public interface IMatcher
    {
        public Result Match(DynamicValue value);
    }

    public delegate Result Matches<in T>(T input);
    
    public class Int : IMatcher
    {
        private readonly Matches<int>[] _expanders;
        public Int(params Matches<int>[] expanders)
        {
            _expanders = expanders;
        }
        public static Matches<int> LowerThan(int boundary) => ((a) => a < boundary);
        public static Matches<int> GreaterThan(int boundary) => ((a) => a > boundary);

        public Result Match(DynamicValue value)
        {
            if (value.Kind == DynamicValueKind.Int)
            {
                var v = (int)value.Value;
                var expanders = _expanders.All(_ => _.Invoke(v));
                return expanders ? Result.Success() : Result.Failure("f");
            };

            return Result.Failure("oops");
        }
    }   
    
    public class Double : IMatcher
    {
        private readonly Matches<double>[] _expanders;
        public Double(params Matches<double>[] expanders)
        {
            _expanders = expanders;
        }
        public static Matches<double> LowerThan(int boundary) => ((a) => a < boundary);
        public static Matches<double> GreaterThan(int boundary) => ((a) => a > boundary);

        public Result Match(DynamicValue value)
        {
            if (value.Kind == DynamicValueKind.Double)
            {
                var expanders = _expanders.All(_ => _.Invoke((double)value.Value));
                return expanders ? Result.Success() : Result.Failure("f");
            };

            return false;
        }
    }   
    
    public class Bool : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Bool;
        }
    }   
    
    public class Null : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Null;
        }
    }   
    
    public class Any : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return Result.Success();
        }
    }
    
    public class Optional : IMatcher
    {
        private readonly IMatcher _inner;

        public Optional(IMatcher inner)
        {
            _inner = inner;
        }
        
        public Result Match(DynamicValue value)
        {
            if (value.Kind == DynamicValueKind.Null)
            {
                return Result.Success();
            }

            return _inner.Match(value);
        }
    }   
    
    public class String : IMatcher
    {
        private readonly Matches<string>[] _expanders;
        public String(params Matches<string>[] expanders)
        {
            _expanders = expanders;
        }
        public static Matches<string> Contains(string seek) => 
            a => a.Contains(seek);
        public static Matches<string> OneOf(params string[] seek) => 
            a => seek.Any(s => s.Equals(a, StringComparison.InvariantCulture));

        public static Matches<string> IsDateTime()
        {
            return a =>
            {
                try
                {
                    var _ = DateTime.Parse(a, CultureInfo.InvariantCulture);
                    return Result.Success();
                }
                catch (FormatException e)
                {
                    return Result.Failure(e.Message);
                }
            };
        }
            

        public Result Match(DynamicValue value)
        {
            if (value.Kind == DynamicValueKind.String)
            {
                var expanders = _expanders.All(_ => _.Invoke((string)value.Value));
                return expanders ? Result.Success() : Result.Failure("f");
            };

            return false;
        }
    }
    
    public class Guid : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Guid;
        }
    }   
    
    public class Array : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Array;
        }
    }   
}

