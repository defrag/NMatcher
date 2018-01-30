using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching.Expanders.Double
{
    public sealed class LowerThan : IDoubleExpander
    {
        private readonly double _boundary;

        public LowerThan(double boundary)
        {
            _boundary = boundary;
        }

        public bool Matches(double value)
        {
            return value < _boundary;
        }
    }
}
