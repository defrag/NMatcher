using System;
using NMatcher.Matching2;
using Array = NMatcher.Matching2.Array;
using Double = NMatcher.Matching2.Double;
using Guid = NMatcher.Matching2.Guid;
using String = NMatcher.Matching2.String;

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

