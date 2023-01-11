using Xunit;

namespace NMatcher.Matching
{
    public class ExpressionMatcherTests
    {
        [Fact]
        public void it_matches_simple_string_expression()
        {
            var matcher = new ExpressionMatcher();

            Assert.True(matcher.MatchExpression("str", "@string@").Successful);
        }
        
        [Fact]
        public void it_matches_simple_int_expression()
        {
            var matcher = new ExpressionMatcher();

            Assert.True(matcher.MatchExpression(1, "@int@").Successful);
        }
    }    
}

