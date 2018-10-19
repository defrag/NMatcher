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
            Assert.True(matcher.MatchExpression("https://amazon.com/dp/1SOTO", "https://@string@.Contains(\"amazon\")/dp/@string@").Successful);
        }

        [Fact]
        public void it_can_match_mixed_expressions()
        {
            var matcher = new Matcher();
            Assert.True(matcher.MatchExpression("https://amazon.com?page=1", "https://@string@.Contains(\"amazon\")?page=@int@").Successful);
        }

        [Fact]
        public void it_can_match_mixed_expressions_with_bool()
        {
            var matcher = new Matcher();
            Assert.True(matcher.MatchExpression("https://amazon.com?isFoo=true", "https://@string@.Contains(\"amazon\")?isFoo=@bool@").Successful);
        }
    }
}
