using System;
using System.Collections.Generic;

namespace NMatcher.Parsing.AST
{
    internal sealed class Expander
    {
        public Expander(string name, IEnumerable<Argument> args)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public string Name { get; }
        public IEnumerable<Argument> Args { get; }
    }
}
