using NMatcher.Matching.Expanders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class StringMatcher : IMatcher
    {
        private readonly IStringExpander[] _expanders;

        public StringMatcher(params IStringExpander[] expanders)
        {
            _expanders = expanders;
        }

        public StringMatcher() : this(new IStringExpander[0])
        {

        }

        public Result Match(object value)
        {
            if (false == value is string)
            {
                return Result.Failure($"{value} is not a valid string.");
            }

            if (_expanders.All(_ => _.Matches((string)value)))
            {
                return Result.Success();
            }

            return Result.Failure($"{value} did not match all the expanders.");
        }
    }
}
