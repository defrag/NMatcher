using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class WildcardTests
    {
        [Fact]
        public void it_matches_everyhing()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("string", "@any@"));
            Assert.True(matcher.MatchExpression(123, "@any@"));
            Assert.True(matcher.MatchExpression(99.99, "@any@"));
            Assert.True(matcher.MatchExpression(false, "@any@"));
        }
    }
}
