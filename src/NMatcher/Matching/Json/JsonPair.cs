using System;

namespace NMatcher.Matching.Json
{
    internal sealed class JsonPair
    {
        public JsonPair(object actual, object expected, string path, bool isEqual)
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
