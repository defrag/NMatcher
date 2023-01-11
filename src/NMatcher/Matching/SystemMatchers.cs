using System;
using System.Globalization;
using System.Linq;

namespace NMatcher.Matching
{
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