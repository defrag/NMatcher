using System;
using NMatcher.Matching;
using Array = NMatcher.Matching.Array;
using Double = NMatcher.Matching.Double;
using Guid = NMatcher.Matching.Guid;
using String = NMatcher.Matching.String;

namespace NMatcher.Activation
{
    internal class Activator
    {
        internal static IMatcher CreateMatcher(Parsing.AST.Type type)
        {
            IMatcher matcher = type.Name switch
            {
                "string" => new String(),
                "double" => new Double(),
                "int" => new Int(),
                "null" => new Null(),
                "bool" => new Bool(),
                "guid" => new Guid(),
                "array" => new Array(),
                "any" => new Any(),
                _ => throw new ArgumentOutOfRangeException($"Type {type.Name} is not supported for matching. ")
            };

            if (type.IsOptional)
            {
                return new Optional(matcher);
            }
            
            return matcher;
        }

        private Matches<string>[] CreateStringExpanders(Parsing.AST.Expander[] expanders)
        {
            var exp = new Matches<string>[expanders.Length];
            for (int i = 0; i < expanders.Length; i++)
            {
                var e = expanders[i];
                // e.Name switch
                // {
                //     
                // }
                //exp[0] =
            }

            return null;
        }
    }
}

