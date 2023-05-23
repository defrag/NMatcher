using System;
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

    public class MatchingException : Exception
    {
        public MatchingException(string msg) : base(msg) 
        {
            
        }
    }
}
