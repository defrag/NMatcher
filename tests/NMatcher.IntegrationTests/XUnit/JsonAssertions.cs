using Xunit;

namespace NMatcher.IntegrationTests.XUnit
{
    public static class JsonAssert
    {
        public static void MatchesJson(string actual, string expected)
        {
            var matcher = new Matcher();
            var rs = matcher.MatchJson(actual, expected);
            Assert.True(rs, rs.ErrorMessage);
        }
        
        public static void NotMatchesJson(string actual, string expected)
        {
            var matcher = new Matcher();
            var rs = matcher.MatchJson(actual, expected);
            Assert.False(rs, rs.ErrorMessage);
        }
    }
}
