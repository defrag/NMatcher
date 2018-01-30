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
            var expJ = JObject.Parse(expected);

            var result = true;

            IterateMatch(actJ.First, node =>
            {

                var regex = new Regex("@[a-zA-Z]+@", RegexOptions.IgnoreCase);
                var exp = expJ.SelectToken(node.Path);

                if (regex.IsMatch(exp.ToString()))
                {
                    var value = (JValue) node.Value;
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

        private void IterateMatch(JToken node, Action<JProperty> action)
        {
            if (node.Type == JTokenType.Object)
            {
                foreach (JProperty child in node.Children<JProperty>())
                {
                    IterateMatch(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    IterateMatch(child, action);
                }
            }
            else
            {
                action((JProperty)node);
            }
        }
    }
}
