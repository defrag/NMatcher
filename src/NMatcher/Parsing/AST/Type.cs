using System;
using System.Collections.Generic;

namespace NMatcher.Parsing.AST
{
    internal sealed class Type : INode
    {
        public Type(string name, IEnumerable<Expander> expanders, bool isOptional = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Expanders = expanders;
            IsOptional = isOptional;
        }

        public string Name { get; }
        public IEnumerable<Expander> Expanders { get; }
        public bool IsOptional { get; }
    }
}
