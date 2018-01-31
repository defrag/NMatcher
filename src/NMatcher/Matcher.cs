using NMatcher.Parsing;
using NMatcher.Activation;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using static NMatcher.Json.JsonTraversal;
using static NMatcher.Json.JsonTokenLoader;
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
            var type = ExpressionParser.ParseExpression(expression);
            var inst = _activator.CreateMatcherInstance(type);

            return inst.Match(value);
        }

        public Result MatchJson(string actual, string expected)
        {
            var actualJson = LoadJson(actual);
            var expectedJson = LoadJson(expected);

            var result = Result.Success();

            TraverseJson(expectedJson, expectedNode =>
            {
                var regex = new Regex("@([a-zA-Z]|\\*)+@", RegexOptions.IgnoreCase);
                var currentNode = (JValue)actualJson.SelectToken(expectedNode.Path);

                if (currentNode == null)
                {
                    result = Result.Failure($"Cound not find corresponding value at path '{expectedNode.Path}'.");
                    return;
                }

                if (regex.IsMatch(expectedNode.ToString()))
                {
                    var r = MatchExpression(currentNode.Value, expectedNode.ToString());
                    if (false == r.Successful)
                    {
                        result = r;
                    } 
                    return;
                }

                if (false == currentNode.Equals(expectedNode))
                {
                    result = Result.Failure($"{currentNode} did not match {expectedNode} at path {expectedNode.Path}.");
                }
            });

            return result;
        }

       
    }
}
