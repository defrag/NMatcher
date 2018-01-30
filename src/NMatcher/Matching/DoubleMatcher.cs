using NMatcher.Matching.Expanders;
using System.Linq;

namespace NMatcher.Matching
{
    public sealed class DoubleMatcher : IMatcher
    {
        private readonly IDoubleExpander[] _expanders;

        public DoubleMatcher(params IDoubleExpander[] expanders)
        {
            _expanders = expanders;
        }

        public DoubleMatcher() : this(new IDoubleExpander[0])
        {

        }

        public Result Match(object value)
        {
            var res = value is float ||
                value is double ||
                value is decimal;

            if (!res)
            {
                return Result.Failure($"{value} is not a valid double.");
            }

            if (_expanders.All(_ => _.Matches((double)value)))
            {
                return Result.Success();
            }

            return Result.Failure($"{value} is not match all the expanders.");
        }
    }
}
