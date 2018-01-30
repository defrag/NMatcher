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

            TraverseJson(actJ, node =>
            {
                var regex = new Regex("@[a-zA-Z]+@", RegexOptions.IgnoreCase);
                var exp = expJ.SelectToken(node.Path);

                if (regex.IsMatch(exp.ToString()))
                {
                    var value = (JValue)actJ.SelectToken(node.Path);

                    if (value == null)
                    {
                        throw new Exception($"Cound not find corresponding value at path '{node.Path}'.");
                    }
                    result = MatchExpression(value.Value, exp.ToString());
                    return;
                }

                if (false == node.Equals(exp))
                {
                    result = false;
                }
            });

            return result;
        }

       
    }
}
