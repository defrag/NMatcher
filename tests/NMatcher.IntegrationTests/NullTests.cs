using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class NullTests
    {
        [Fact]
        public void it_matches_null()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(null, "@null@"));
        }
    }
}
