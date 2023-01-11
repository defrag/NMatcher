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

        [Fact] 
        public void it_catches_activation_exception_and_returns_the_failed_result()
        {
            var matcher = new ExpressionMatcher();
            var rs = matcher.MatchExpression(1, "@faoo@");
            Assert.False(rs.Successful);
            Assert.Equal("Type faoo is not supported for matching.", rs.ErrorMessage);
        }
        
        [Fact] 
        public void it_catches_activation_exception_for_expanders()
        {
            var matcher = new ExpressionMatcher();
            var rs = matcher.MatchExpression(1, "@int@.IsDateTime()");
            Assert.False(rs.Successful);
            Assert.Equal("Expander IsDateTime is not supported for type int.", rs.ErrorMessage);
        }
    }    
}

