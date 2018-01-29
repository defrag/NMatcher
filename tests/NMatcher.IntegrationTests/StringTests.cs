using Xunit;

namespace NMatcher.IntegrationTests
{
    public class StringTests
    {
        [Fact]
        public void it_matches_string()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("str", "@string@"));
        }

        [Fact]
        public void it_matches_string_with_contains()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("string", "@string@.Contains(\"str\")"));
            Assert.True(matcher.MatchExpression("string", "@string@.Contains('str')"));
            Assert.False(matcher.MatchExpression("barbaz", "@string@.Contains(\"str\")"));
        }
    }
}
