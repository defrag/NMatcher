using NMatcher.Activation;
using NMatcher.Parsing;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using NMatcher.Matching;

namespace NMatcher.Matching2
{
    public class ExpressionMatcher2 
    {

        internal static readonly Regex MatcherRegex = new Regex("@([a-zA-Z\\?])+@", RegexOptions.IgnoreCase);
        

        public Result MatchExpression(object value, string expression)
        {
            var expressions = ExpressionParser.ParseExpressions(expression).ToList();
            
            if (expressions.Count == 1 || value is null)
            {
                return expressions
                    .OfType<Parsing.AST.Type>()
                    .Select(Activator.CreateMatcher)
                    .First()
                    .Match(DynamicValue.UnsafelyTryCreateFrom(value));
            }
            
            return Result.Success();
        }



        private static string NodeToRegex(Parsing.AST.INode node)
        {
            switch (node)
            {
                case Parsing.AST.Literal l: return $"(?:{Regex.Escape(l.ToString())})";
                default: return $"(.*)";
            }
        }

    }
}
