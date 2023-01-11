using NMatcher.Matching;

namespace NMatcher
{
    public class Matcher
    {
        public Result MatchExpression(object value, string expression)
        {
            return new ExpressionMatcher().MatchExpression(value, expression);
        }

        public Result MatchJson(string actual, string expected)
        {
            return new JsonMatcher(new ExpressionMatcher(), expected).Match(new DynamicValue(actual, DynamicValueKind.String));
        }
    }
}
