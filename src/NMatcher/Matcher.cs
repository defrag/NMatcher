using NMatcher.Activation;
using NMatcher.Matching;
using NMatcher.Matching2;

namespace NMatcher
{
    public class Matcher
    {
        public Result MatchExpression(object value, string expression)
        {
            return new ExpressionMatcher2().MatchExpression(value, expression);
        }

        public Result MatchJson(string actual, string expected)
        {
            return new JsonMatcher(new ExpressionMatcher(), expected).Match(actual);
        }
    }
}
