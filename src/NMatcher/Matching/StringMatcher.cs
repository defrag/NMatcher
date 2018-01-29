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

        public bool Match(object value)
        {
            return value is string && _expanders.All(_ => _.Matches((string)value));
        }
    }
}
