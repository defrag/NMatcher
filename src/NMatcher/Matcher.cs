using NMatcher.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using NMatcher.Activation;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using static NMatcher.Json.JsonTraversal;

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

        public bool MatchExpression(object value, string expression)
        {
            var t = value.GetType(); ;
            var type = ExpressionParser.ParseExpression(expression);
            var inst = _activator.CreateMatcherInstance(type);

            return inst.Match(value);
        }

        public bool MatchJson(string actual, string expected)
        {
            var actJ = JToken.Parse(actual);
            var expJ = JToken.Parse(expected);

            var result = true;

            TraverseJson(expJ, expectedNode =>
            {
                var regex = new Regex("@[a-zA-Z]+@", RegexOptions.IgnoreCase);
                var currentNode = (JValue)actJ.SelectToken(expectedNode.Path);

                if (currentNode == null)
                {
                    throw new Exception($"Cound not find corresponding value at path '{expectedNode.Path}'.");
                }

                if (regex.IsMatch(expectedNode.ToString()))
                {
                    result = MatchExpression(currentNode.Value, expectedNode.ToString());
                    return;
                }

                if (false == currentNode.Equals(expectedNode))
                {
                    result = false;
                }
            });

            return result;
        }

       
    }
}
