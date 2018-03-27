using System;
using System.Collections.Generic;
using System.Linq;

namespace NMatcher.Matching.Expanders.String
{
    public sealed class OneOf : IStringExpander
    {
        private readonly IEnumerable<string> _choices;

        public OneOf(params string[] choices)
        {
            if (null == choices)
            {
                throw new ArgumentNullException(nameof(choices));
            }

            if (choices.Length < 2)
            {
                throw new ArgumentException($"OneOf expander expects at least two choices supplied, but {choices.Length} given.");
            }

            _choices = choices;
        }

        public bool Matches(string value)
        {
            return _choices.Any(_ => _ == value);
        }
    }
}
