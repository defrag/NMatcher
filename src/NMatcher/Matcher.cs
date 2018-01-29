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
            var type = ExpressionParser.ParseExpression(expression);
            var inst = _activator.CreateMatcherInstance(type);

            return inst.Match(value);
        }

        public bool MatchJson(string actual, string expected)
        {
            var actJ = JObject.Parse(actual);
            var expJ = JObject.Parse(expected);

            foreach (JProperty x in (JToken)actJ)
            {
                var act = x.Value;    
                var exp = expJ.SelectToken(x.Path);

                var regex = new Regex("@[a-zA-Z]+@", RegexOptions.IgnoreCase);
                
                if (regex.IsMatch(exp.ToString()))
                {
                    var result = MatchExpression(act.ToString(), exp.ToString());
                    if (false == result)
                    {
                        return false;
                    }

                    continue;
                }

                if (false == act.Equals(exp))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
