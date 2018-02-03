using Xunit;

namespace NMatcher.IntegrationTests
{
    public class StringTests
    {
        [Fact]
        public void it_matches_string()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("str", "@string@").Successful);
        }

        [Fact]
        public void it_matches_string_with_optional()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(null, "@string?@").Successful);
        }

        [Fact]
        public void it_matches_string_with_contains()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("string", "@string@.Contains(\"str\")"));
            Assert.True(matcher.MatchExpression("string", "@string@.Contains('str')"));
            Assert.False(matcher.MatchExpression("barbaz", "@string@.Contains(\"str\")"));
        }

        [Fact]
        public void it_matches_string_which_is_date()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("2018-01-01", "@string@.IsDateTime()"));
            Assert.False(matcher.MatchExpression("test", "@string@.IsDateTime()"));
        }

        [Fact]
        public void it_matches_string_which_is_datetime()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("2018-01-01 11:00:12", "@string@.IsDateTime()"));
        }

        [Fact]
        public void it_returns_false_when_value_is_not_string()
        {
            var matcher = new Matcher();
            var result = matcher.MatchExpression(100, "@string@.Contains(\"str\")");
            Assert.False(result.Successful);
            Assert.Equal("100 is not a valid string.", result.ErrorMessage);
        }
    }
}
