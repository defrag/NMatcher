using System;

namespace NMatcher.Parsing.AST
{
    internal sealed class Argument
    {
        public Argument(object value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public object Value { get; }
    }
}
