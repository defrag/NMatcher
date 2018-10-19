using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class CompoundTests
    {
        [Fact]
        public void it_can_match_mutltiple_string_expressions_divided_by_literals()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("https://amazon.com/dp/11SOTO", "https://@string@.Contains(\"amazon\")/dp/@string@").Successful);
            Assert.True(matcher.MatchExpression("https://amazon.com?filter=foo", "https://@string@.Contains(\"amazon\")?page=@string@").Successful);
        }
    }
}
