using NMatcher.Activation;
using NMatcher.Matching;

namespace NMatcher
{
    public class Matcher
    {
        private readonly IActivator _activator;

        public Matcher(IActivator activator)
        {
            _activator = activator;
        }

        public Matcher() : this(new DefaultActivator())
        {

        }

        public Result MatchExpression(object value, string expression)
        {
            return new ExpressionMatcher(_activator).MatchExpression(value, expression);
        }

        public Result MatchJson(string actual, string expected)
        {
            return new JsonMatcher(new ExpressionMatcher(_activator), expected).Match(actual);
        }
        
        public Result MatchJson2(string actual, string expected)
        {
            return new JsonMatcher(new ExpressionMatcher(_activator), expected).Match(actual);
        }
    }
}
