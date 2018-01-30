using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class BoolMatcher : IMatcher
    {
        public Result Match(object value)
        {
            var res = value is bool;

            return res ? Result.Success() : Result.Failure($"{value} is not a valid bool.");
        }
    }
}
