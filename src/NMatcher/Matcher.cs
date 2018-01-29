using NMatcher.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace NMatcher
{
    public class Matcher
    {
        public bool MatchExpression(object value, string expression)
        {
            var parseResult = ExpressionParser.Parse(expression);
            return true;
        }
    }
}
