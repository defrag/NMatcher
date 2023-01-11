﻿using System.Linq;
using System.Text.RegularExpressions;
using NMatcher.Activation;
using NMatcher.Parsing;

namespace NMatcher.Matching
{
    public class ExpressionMatcher 
    {
        internal static readonly Regex MatcherRegex = new Regex("@([a-zA-Z\\?])+@", RegexOptions.IgnoreCase);
        
        public Result MatchExpression(object value, string expression)
        {
            try
            {
                var expressions = ExpressionParser.ParseExpressions(expression).ToList();

                if (expressions.Count == 1 || value is null)
                {
                    return expressions
                        .OfType<Parsing.AST.Type>()
                        .Select(Activator.CreateMatcher)
                        .First()
                        .Match(DynamicValue.Create(value));
                }

                var regex = new Regex(string.Join("", expressions.Select(NodeToRegex)));

                if (false == regex.IsMatch(value.ToString()))
                {
                    return Result.Failure($"Value {value} does not match expression {expression}.");
                }

                var results = regex
                    .Match(value.ToString())
                    .Groups
                    .Cast<Group>()
                    .Skip(1)
                    .Select(_ => ExpressionParser.ParseLiteral(_.ToString()))
                    .Select(_ => _.Value);

                var parts = expressions
                    .OfType<Parsing.AST.Type>()
                    .Select(Activator.CreateMatcher)
                    .Zip(results, (m, v) => m.Match(DynamicValue.Create(v)));

                return parts.FirstOrDefault(_ => false == _.Successful) ?? Result.Success();
            }
            catch (ActivationException e)
            {
                return Result.Failure(e.Message);
            }
            
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
