using NMatcher.Activation;
using NMatcher.Parsing;
using System;
using System.Text.RegularExpressions;

namespace NMatcher.Matching
{
    public class ExpressionMatcher
    {
        private readonly IActivator _activator;
        internal static readonly Regex MatcherRegex = new Regex("@([a-zA-Z\\?])+@", RegexOptions.IgnoreCase);

        public ExpressionMatcher(IActivator activator)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        public ExpressionMatcher() : this(new DefaultActivator())
        {

        }

        public Result MatchExpression(object value, string expression)
        {
            var type = ExpressionParser.ParseExpression(expression);
            var inst = _activator.CreateMatcherInstance(type);

            return inst.Match(value);
        }
    }
}
