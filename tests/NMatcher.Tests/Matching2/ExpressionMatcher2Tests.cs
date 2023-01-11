using Xunit;

namespace NMatcher.Matching2
{
    public class ExpressionMatcher2Tests
    {
        [Fact]
        public void it_matches_simple_string_expression()
        {
            var matcher = new ExpressionMatcher2();

            Assert.True(matcher.MatchExpression("str", "@string@").Successful);
        }
        
        [Fact]
        public void it_matches_simple_int_expression()
        {
            var matcher = new ExpressionMatcher2();

            Assert.True(matcher.MatchExpression(1, "@int@").Successful);
        }
    }    
}

