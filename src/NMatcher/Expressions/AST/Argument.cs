using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Expressions.AST
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
