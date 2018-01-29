using System;

namespace NMatcher.Parsing.AST
{
    public sealed class Argument
    {
        public Argument(object value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public object Value { get; }
    }
}
