using Xunit;

namespace NMatcher.IntegrationTests
{
    public class DoubleTests
    {
        [Fact]
        public void it_matches_float_double_and_decimals()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(100.00, "@double@"));
            Assert.True(matcher.MatchExpression(100.0f, "@double@"));
            Assert.True(matcher.MatchExpression(100.0M, "@double@"));
        }
    }
}
