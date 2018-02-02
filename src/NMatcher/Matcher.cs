using NMatcher.Parsing;
using NMatcher.Activation;
using System.Text.RegularExpressions;
using NMatcher.Matching;
using System.Linq;
using NMatcher.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NMatcher.Json.Pairing;

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

            var result = JsonMatcher.IterateMatch(actual, expected);

            var expectedDiff = result.Item2.Except(result.Item3);
            if (expectedDiff.Any())
            {
                return Result.Failure($"Expected value did not appear at path {expectedDiff.First()}.");
            }

            var actualDiff = result.Item3.Except(result.Item2);
            if (actualDiff.Any())
            {
                return Result.Failure($"Actual value did not appear at path {actualDiff.First()}.");
            }

            var error = result.Item1
                   .Select(ToPairingResult)
                   .FirstOrDefault(_ => false == _.Successful);

            return error ?? Result.Success();
        }
    }
}
