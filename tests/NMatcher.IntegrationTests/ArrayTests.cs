using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class ArrayTests
    {
        [Fact]
        public void it_matches_array()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(new int[] { 1, 2, 3 }, "@array@"));
            Assert.True(matcher.MatchExpression(new string[] { "fuu", "bar", "baz" }, "@array@"));
        }
    }
}
