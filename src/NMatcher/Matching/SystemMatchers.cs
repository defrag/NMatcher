using System;
using System.Globalization;
using System.Linq;

namespace NMatcher.Matching
{
    internal sealed class Int : IMatcher
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
                return expanders ? Result.Success() :Result.Failure($"{value} did not match all expanders.");
            };

            return Result.Failure($"{value} is not a valid int.");
        }
    }   
    
    internal sealed  class Double : IMatcher
    {
        private readonly Matches<double>[] _expanders;
        public Double(params Matches<double>[] expanders)
        {
            _expanders = expanders;
        }
        public static Matches<double> LowerThan(double boundary) => ((a) => a < boundary);
        public static Matches<double> GreaterThan(double boundary) => ((a) => a > boundary);

        public Result Match(DynamicValue value)
        {
            if (value.Kind == DynamicValueKind.Double)
            {
                var expanders = _expanders.All(_ => _.Invoke((double)value.Value));
                return expanders ? Result.Success() : Result.Failure($"{value} did not match all expanders.");
            };

            return Result.Failure($"{value} is not a valid double.");
        }
    }   
    
    internal sealed class Bool : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Bool
                ? Result.Success()
                : Result.Failure($"{value} is not a valid bool.");;
        }
    }   
    
    internal sealed class Null : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Null
                ? Result.Success()
                : Result.Failure($"{value} is not a valid null.");;
        }
    }   
    
    internal sealed class Any : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return Result.Success();
        }
    }
    
    internal sealed class Optional : IMatcher
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
    
    internal sealed class String : IMatcher
    {
        private readonly Matches<string>[] _expanders;
        public String(params Matches<string>[] expanders)
        {
            _expanders = expanders;
        }
        public static Matches<string> Contains(string seek) => 
            a => a.Contains(seek);

        public static Matches<string> OneOf(params string[] seek)
        {
            if (seek.Length < 2)
            {
                throw new ArgumentException($"OneOf expander expects at least two choices supplied, but {seek.Length} given.");
            }
            
            return a => 
                seek.Any(s => s.Equals(a, StringComparison.InvariantCulture));
        }
            
        
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
                return expanders ? Result.Success() : Result.Failure($"{value} id not match all expanders.");
            };

            return Result.Failure($"{value} is not a valid string.");
        }
    }
    
    internal sealed class Guid : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Guid
                ? Result.Success()
                : Result.Failure($"{value} is not a valid guid.");;
        }
    }   
    
    internal sealed class Array : IMatcher
    {
        public Result Match(DynamicValue value)
        {
            return value.Kind == DynamicValueKind.Array
                ? Result.Success()
                : Result.Failure($"{value} is not a valid array.");
        }
    }   
}