using NMatcher.Matching.Expanders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class IntMatcher : IMatcher
    {
        private readonly IIntExpander[] _expanders;

        public IntMatcher(params IIntExpander[] expanders)
        {
            _expanders = expanders;
        }

        public IntMatcher() : this(new IIntExpander[0])
        {

        }

        public Result Match(object value)
        {
            var res = value is short || 
                value is ushort ||
                value is int ||
                value is uint ||
                value is long ||
                value is ulong;

            if (!res)
            {
                return Result.Failure($"{value} ({value.GetType()}) is not a valid int.");
            }

            if (_expanders.All(_ => _.Matches((int)value)))
            {
                return Result.Success();
            }

            return Result.Failure($"{value} did not match all the expanders.");
        }
    }
}
