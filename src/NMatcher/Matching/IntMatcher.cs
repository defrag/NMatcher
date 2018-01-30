using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class IntMatcher : IMatcher
    {
        public Result Match(object value)
        {
            var res = value is short || 
                value is ushort ||
                value is int ||
                value is uint ||
                value is long ||
                value is ulong;

            return res ? Result.Success() : Result.Failure($"{value} is not a valid int.");
        }
    }
}
