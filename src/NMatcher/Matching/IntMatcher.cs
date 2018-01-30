using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class IntMatcher : IMatcher
    {
        public bool Match(object value)
        {
            return value is short || 
                value is ushort ||
                value is int ||
                value is uint ||
                value is long ||
                value is ulong;
        }
    }
}
