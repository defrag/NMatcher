using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMatcher.Matching;
using NMatcher.Matching.Expanders.String;

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
                        { "Contains", typeof(Contains) },
                        { "IsDateTime", typeof(IsDateTime) }
                    } 
                )
            },
            {
                "int",
                new Definition(
                    typeof(IntMatcher)
                )
            },
            {
                "double",
                new Definition(
                    typeof(DoubleMatcher)
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
