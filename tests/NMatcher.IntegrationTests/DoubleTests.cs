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
        public void it_matches_double_with_optional()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(null, "@double?@"));
        }

        [Fact]
        public void it_matches_with_greater_than()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(17.59, "@double@.GreaterThan(17.50)"));
            Assert.False(matcher.MatchExpression(5.5, "@double@.GreaterThan(10.0)"));
        }

        [Fact]
        public void it_matches_with_lower_than()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(9.5, "@double@.LowerThan(10.0)"));
            Assert.False(matcher.MatchExpression(5.5, "@double@.LowerThan(1.0)"));
        }

        [Fact]
        public void it_returns_false_when_value_is_not_double()
        {
            var matcher = new Matcher();
            var result = matcher.MatchExpression("fuuu", "@double@");
            Assert.False(result.Successful);
            Assert.Equal("\"fuuu\" (String) is not a valid double.", result.ErrorMessage);
        }
    }
}
