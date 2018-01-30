using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class IntTests
    {
        [Fact]
        public void it_matches_ints_properly()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(100, "@int@").Successful);
        }

        [Fact]
        public void it_doesnt_match_int_strings()
        {
            var matcher = new Matcher();

            Assert.False(matcher.MatchExpression("1000", "@int@").Successful);
        }

        [Fact]
        public void it_returns_false_when_value_is_not_int()
        {
            var matcher = new Matcher();
            var result = matcher.MatchExpression(99.99, "@int@");
            Assert.False(result.Successful);
            Assert.Equal("99.99 is not a valid int.", result.ErrorMessage);
        }
    }
}
