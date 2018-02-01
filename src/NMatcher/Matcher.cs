using NMatcher.Parsing;
using NMatcher.Activation;
using System.Text.RegularExpressions;
using static NMatcher.Json.JsonComparator;
using NMatcher.Matching;

namespace NMatcher
{
    public class Matcher
    {
        private readonly Regex _matcherRegex = new Regex("@([a-zA-Z]|\\*)+@", RegexOptions.IgnoreCase);
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
            var type = ExpressionParser.ParseExpression(expression);
            var inst = _activator.CreateMatcherInstance(type);

            return inst.Match(value);
        }

        public Result MatchJson(string actual, string expected)
        {
            var result = Result.Success();

            string FormatError(object act, object exp, string path)
            {
                var ac = act == null ? "null" : act.ToString();
                var ec = exp == null ? "null" : exp.ToString();

                if (act == null && exp != null)
                {
                    return $"Actual value did not appear at path {path}.";
                }

                if (act != null && exp == null)
                {
                    return $"Expected value did not appear at path {path}.";
                }

                return $"{ac} did not match {ec} at path {path}.";
            };

            CompareJson(actual, expected, (a, e, path, compareFn) =>
            {
                if (e != null && _matcherRegex.IsMatch(e.ToString()))
                {
                    var r = MatchExpression(a, e.ToString());
                    if (false == r.Successful)
                    {
                        result = r;
                    } 
                    return;
                }

                if (false == compareFn())
                {
                    result = Result.Failure(FormatError(a, e, path));
                }
            });

            return result;
        }

       
    }
}
