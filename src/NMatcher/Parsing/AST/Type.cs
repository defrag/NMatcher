using System;
using System.Collections.Generic;

namespace NMatcher.Parsing.AST
{
    public sealed class Type
    {
        public Type(string name, IEnumerable<Expander> expanders)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Expanders = expanders;
        }

        public string Name { get; }
        public IEnumerable<Expander> Expanders { get; }
    }
}
