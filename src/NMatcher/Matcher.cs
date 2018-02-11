using NMatcher.Parsing;
using NMatcher.Activation;
using System.Text.RegularExpressions;
using NMatcher.Matching;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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

        public Result MatchXml(string actual, string expected)
        {
            return new XmlMatcher(new ExpressionMatcher(_activator), expected).Match(actual);
        }
    }
}
