using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Json.Pairing
{
    internal sealed class Pair
    {
        public Pair(object actual, object expected, string path, bool isEqual)
        {
            Actual = actual;
            Expected = expected;
            Path = path ?? throw new ArgumentNullException(nameof(path));
            IsEqual = isEqual;
        }

        public object Actual { get; }
        public object Expected { get; }
        public string Path { get; }
        public bool IsEqual { get; }
    }
}
