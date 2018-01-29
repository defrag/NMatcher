using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Expressions.AST
{
    public sealed class Expander
    {
        public Expander(string name, IEnumerable<Argument> args)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Args = args;
        }

        public string Name { get; }
        public IEnumerable<Argument> Args { get; }
    }
}
