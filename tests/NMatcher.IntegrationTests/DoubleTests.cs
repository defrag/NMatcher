using Xunit;

namespace NMatcher.IntegrationTests
{
    public class DoubleTests
    {
        [Fact]
        public void it_matches_float_double_and_decimals()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(100.00, "@double@").Successful);
            Assert.True(matcher.MatchExpression(100.0f, "@double@").Successful);
            Assert.True(matcher.MatchExpression(100.0M, "@double@").Successful);
        }

        [Fact]
        public void it_returns_false_when_value_is_not_double()
        {
            var matcher = new Matcher();
            var result = matcher.MatchExpression("fuuu", "@double@");
            Assert.False(result.Successful);
            Assert.Equal("fuuu is not a valid double.", result.ErrorMessage);
        }
    }
}
