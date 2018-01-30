using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class WildcardMatcher : IMatcher
    {
        public Result Match(object value)
        {
            return Result.Success();
        }
    }
}
