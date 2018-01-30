using NMatcher.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMatcher.Activation;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using static NMatcher.Json.JsonTraversal;
using NMatcher.Matching;
using Newtonsoft.Json;
using System.IO;

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
            JToken LoadToken(string s)
            {
                JsonReader reader = new JsonTextReader(new StringReader(s));
                reader.DateParseHandling = DateParseHandling.None;
                return JToken.Load(reader);
            }
            
            var actJ = LoadToken(actual);
            var expJ = LoadToken(expected);

            var result = Result.Success();

            TraverseJson(expJ, expectedNode =>
            {
                var regex = new Regex("@([a-zA-Z]|\\*)+@", RegexOptions.IgnoreCase);
                var currentNode = (JValue)actJ.SelectToken(expectedNode.Path);

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
                    result = Result.Failure($"Node {currentNode} did not match {expectedNode}.");
                }
            });

            return result;
        }

       
    }
}
