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

            Assert.True(matcher.MatchExpression(100, "@int@"));
        }

        [Fact]
        public void it_doesnt_match_int_strings()
        {
            var matcher = new Matcher();

            Assert.False(matcher.MatchExpression("1000", "@int@"));
        }
    }
}
