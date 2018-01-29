using System;

namespace NMatcher.Matching.Expanders.String
{
    public sealed class Contains : IStringExpander
    {
        private readonly string _needle;

        public Contains(string needle)
        {
            _needle = needle ?? throw new ArgumentNullException(nameof(needle));
        }

        public bool Matches(string value)
        {
            return value.Contains(_needle);
        }
    }
}
