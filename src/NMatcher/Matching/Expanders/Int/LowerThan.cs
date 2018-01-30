using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching.Expanders.Int
{
    public sealed class LowerThan : IIntExpander
    {
        private readonly int _boundary;

        public LowerThan(int boundary)
        {
            _boundary = boundary;
        }

        public bool Matches(int value)
        {
            return value < _boundary;
        }
    }
}
