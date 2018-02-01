using NMatcher.Parsing;
using NMatcher.Activation;
using System.Text.RegularExpressions;
using NMatcher.Json.Pairing;
using NMatcher.Matching;
using System.Linq;
using NMatcher.Json.Pairing.Exceptions;

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
            Result ToPairingResult(Pair pair)
            {
                if (pair.Expected != null && _matcherRegex.IsMatch(pair.Expected.ToString()))
                {
                    return MatchExpression(pair.Actual, pair.Expected.ToString());
                }

                return pair.IsEqual ? Result.Success() : Result.Failure(FormatError(pair.Actual, pair.Expected, pair.Path));
            };

            string FormatError(object act, object exp, string path)
            {
                var ac = act == null ? "null" : act.ToString();
                var ec = exp == null ? "null" : exp.ToString();

                return $"{ac} did not match {ec} at path {path}.";
            };

            try
            {
                var error = JsonPairing.PairJson(actual, expected)
                    .Select(ToPairingResult)
                    .FirstOrDefault(_ => false == _.Successful);

                return error ?? Result.Success();
            }
            catch (PathMissingException e)
            {
                return Result.Failure(e.Message);
            }
        }
    }
}
