using Xunit;

namespace NMatcher.IntegrationTests
{
    public class GuidTests
    {
        [Fact]
        public void it_matches_guids_properly()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("843475f5-f7c9-4a28-b028-a3a7dc456e91", "@guid@"));
            Assert.True(matcher.MatchExpression("C56A4180-65AA-42EC-A945-5FD21DEC0538", "@guid@"));
        }

        [Fact]
        public void it_matches_guid_with_optional()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(null, "@guid?@"));
        }

        [Fact]
        public void it_returns_false_when_value_is_not_guid()
        {
            var matcher = new Matcher();
            var result = matcher.MatchExpression("fuuuu", "@guid@");
            Assert.False(result.Successful);
            Assert.Equal("fuuuu is not a valid Guid.", result.ErrorMessage);
        }
    }
}
