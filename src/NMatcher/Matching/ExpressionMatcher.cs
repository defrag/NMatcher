using NMatcher.Activation;
using NMatcher.Parsing;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NMatcher.Matching
{
    public class ExpressionMatcher
    {
        private readonly IActivator _activator;

        internal static readonly Regex MatcherRegex = new Regex("@([a-zA-Z\\?])+@", RegexOptions.IgnoreCase);

        private ExpressionMatcher(IActivator activator)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        public ExpressionMatcher() : this(new DefaultActivator())
        {

        }

        public Result MatchExpression(object value, string expression)
        {
            var expressions = ExpressionParser.ParseExpressions(expression);

            if (expressions.Count() == 1 || value is null)
            {
                return expressions
                    .OfType<Parsing.AST.Type>()
                    .Select(_activator.CreateMatcherInstance)
                    .First()
                    .Match(value);
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
                .Select(_activator.CreateMatcherInstance)
                .Zip(results, (m, v) => m.Match(v));

            return parts.FirstOrDefault(_ => false == _.Successful) ?? Result.Success(); 
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
