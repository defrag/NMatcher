using System;
using System.Collections.Generic;
using System.Linq;
using NMatcher.Matching;
using Array = NMatcher.Matching.Array;
using Double = NMatcher.Matching.Double;
using Guid = NMatcher.Matching.Guid;
using String = NMatcher.Matching.String;

namespace NMatcher.Activation
{
    internal sealed class ActivationException : Exception
    {
        public ActivationException(string message) : base(message)
        {
        }
    }
    
    internal static class Activator
    {
        internal static IMatcher CreateMatcher(Parsing.AST.Type type)
        {
            IMatcher matcher = type.Name switch
            {
                "string" => new String(CreateStringExpanders(type.Expanders)),
                "double" => new Double(CreateDoubleExpanders(type.Expanders)),
                "int" => new Int(CreateIntExpanders(type.Expanders)),
                "null" => new Null(),
                "bool" => new Bool(),
                "guid" => new Guid(),
                "array" => new Array(),
                "any" => new Any(),
                _ => throw new ActivationException($"Type {type.Name} is not supported for matching.")
            };

            if (type.IsOptional)
            {
                return new Optional(matcher);
            }
            
            return matcher;
        }

        private static Matches<string>[] CreateStringExpanders(IReadOnlyCollection<Parsing.AST.Expander> expandersIn)
        {
            var expanders = expandersIn.ToArray();
            var exp = new Matches<string>[expanders.Length];
            for (int i = 0; i < expanders.Length; i++)
            {
                var e = expanders[i];
                var matches = e.Name switch
                {
                    "Contains" => String.Contains((string)e.Args.First().Value),
                    "IsDateTime" => String.IsDateTime(),
                    "OneOf" => String.OneOf(e.Args.Select(v => (string)v.Value).ToArray()),
                    _ => throw new ActivationException($"Expander {e.Name} is not supported for type string.")
                };
                exp[0] = matches;
            }

            return exp;
        }
        
        private static Matches<int>[] CreateIntExpanders(IReadOnlyCollection<Parsing.AST.Expander> expandersIn)
        {
            var expanders = expandersIn.ToArray();
            var exp = new Matches<int>[expanders.Length];
            for (int i = 0; i < expanders.Length; i++)
            {
                var e = expanders[i];
                var matches = e.Name switch
                {
                    "GreaterThan" => Int.GreaterThan((int)e.Args.First().Value),
                    "LowerThan" => Int.LowerThan((int)e.Args.First().Value),
                    _ => throw new ActivationException($"Expander {e.Name} is not supported for type int.")
                };
                exp[0] = matches;
            }

            return exp;
        }
        
        private static Matches<double>[] CreateDoubleExpanders(IReadOnlyCollection<Parsing.AST.Expander> expandersIn)
        {
            var expanders = expandersIn.ToArray();
            var exp = new Matches<double>[expanders.Length];
            for (int i = 0; i < expanders.Length; i++)
            {
                var e = expanders[i];
                var matches = e.Name switch
                {
                    "GreaterThan" => Double.GreaterThan((double)e.Args.First().Value),
                    "LowerThan" => Double.LowerThan((double)e.Args.First().Value),
                    _ => throw new ActivationException($"Expander {e.Name} is not supported for type double.")
                };
                exp[0] = matches;
            }

            return exp;
        }
    }
}

