using System;

namespace NMatcher.Parsing.AST
{
    public sealed class Literal : INode
    {
        public Literal(string value, LiteralType type)
        {
            var rawValue = TryGetObjectValue(value, type);
            Value = rawValue;
            Type = type;
        }

        public object Value { get; }
        public LiteralType Type { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public enum LiteralType
        {
            String,
            Int,
            Double,
            Bool
        }

        private static object TryGetObjectValue(string value, LiteralType type)
        {
            switch (type)
            {
                case LiteralType.String: return value;
                case LiteralType.Int: return int.Parse(value);
                case LiteralType.Bool: return bool.Parse(value);
                case LiteralType.Double: return double.Parse(value);
            }

            throw new InvalidOperationException($"Cannot get literal value of type {type}.");
        }
    }
}
