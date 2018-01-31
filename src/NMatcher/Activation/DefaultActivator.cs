using System;
using System.Collections.Generic;
using System.Linq;
using NMatcher.Matching;
using Ints = NMatcher.Matching.Expanders.Int;
using Strings = NMatcher.Matching.Expanders.String;
using Doubles = NMatcher.Matching.Expanders.Double;

namespace NMatcher.Activation
{
    public sealed class DefaultActivator : IActivator
    {
        private IDictionary<string, Definition> _map = new Dictionary<string, Definition>()
        {
            {
                "string",
                new Definition(
                    typeof(StringMatcher),
                    new Dictionary<string, Type>()
                    {
                        { "Contains", typeof(Strings.Contains) },
                        { "IsDateTime", typeof(Strings.IsDateTime) }
                    } 
                )
            },
            {
                "int",
                new Definition(
                    typeof(IntMatcher),
                    new Dictionary<string, Type>()
                    {
                        { "GreaterThan", typeof(Ints.GreaterThan) },
                        { "LowerThan", typeof(Ints.LowerThan) }
                    }
                )
            },
            {
                "double",
                new Definition(
                    typeof(DoubleMatcher),
                    new Dictionary<string, Type>()
                    {
                        { "GreaterThan", typeof(Doubles.GreaterThan) },
                        { "LowerThan", typeof(Doubles.LowerThan) }
                    }
                )
            },
            {
                "bool",
                new Definition(
                    typeof(BoolMatcher)
                )
            },
            {
                "any",
                new Definition(
                    typeof(WildcardMatcher)
                )
            },
            {
                "null",
                new Definition(
                    typeof(NullMatcher)
                )
            },
            {
                "guid",
                new Definition(
                    typeof(GuidMatcher)
                )
            }
        };

        public IMatcher CreateMatcherInstance(Parsing.AST.Type type)
        {
            if (false == _map.ContainsKey(type.Name))
            {
                throw new Exception($"Type {type.Name} could not be found in map.");
            }

            var definition = _map[type.Name];

            var expanders = type.Expanders
               .Select(_ => Tuple.Create(
                   definition.Expanders[_.Name], 
                   _.Args.Select(a => a.Value))
               )
               .Select(_ => Activator.CreateInstance(_.Item1, _.Item2.ToArray()) );

            return (IMatcher) Activator.CreateInstance(definition.Type, expanders.ToArray());
        }
    }
}
