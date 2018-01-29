using System;
using System.Collections.Generic;

namespace NMatcher.Activation
{
    public sealed class Definition
    {
        public Definition(Type type, IDictionary<string, Type> expanders)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Expanders = expanders ?? throw new ArgumentNullException(nameof(expanders));
        }

        public Definition(Type type) : this(type, new Dictionary<string, Type>())
        {

        }

        public Type Type { get; }
        public IDictionary<string, Type> Expanders { get; }
    }
}
