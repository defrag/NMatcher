using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class BoolTests
    {
        [Fact]
        public void it_matches_bools()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(true, "@bool@"));
            Assert.True(matcher.MatchExpression(false, "@bool@"));
        }

        [Fact]
        public void it_matches_bools_with_optional()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression(null, "@bool?@"));
        }

        [Fact]
        public void it_returns_false_when_value_is_not_double()
        {
            var matcher = new Matcher();
            var result = matcher.MatchExpression("fuuu", "@bool@");
            Assert.False(result.Successful);
            Assert.Equal("Value fuuu of Kind String is not a valid bool.", result.ErrorMessage);
        }
    }
}
