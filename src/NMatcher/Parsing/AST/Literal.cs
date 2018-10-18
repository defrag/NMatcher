using System;

namespace NMatcher.Parsing.AST
{
    public sealed class Literal : INode
    {
        public Literal(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}
