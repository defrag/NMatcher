using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Expressions.AST
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
