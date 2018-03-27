using System;
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


        [Fact]
        public void it_matches_one_of_choices()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchExpression("foobar", "@string@.OneOf('foobar', 'baz')"));
            Assert.True(matcher.MatchExpression("baz", "@string@.OneOf('foobar', 'baz')"));
            Assert.False(matcher.MatchExpression("bearwalk", "@string@.OneOf('foobar', 'baz')"));
        }

        [Fact]
        public void it_throws_exception_when_no_choices_were_provided()
        {
            var matcher = new Matcher();
            var exception = Assert.Throws<ArgumentException>(() => matcher.MatchExpression("baz", "@string@.OneOf()"));
            Assert.Equal("OneOf expander expects at least two choices supplied, but 0 given.", exception.Message);
        }

        [Fact]
        public void it_throws_exception_when_only_one_choice_was_provided()
        {
            var matcher = new Matcher();
            var exception = Assert.Throws<ArgumentException>(() => matcher.MatchExpression("baz", "@string@.OneOf('bar')"));
            Assert.Equal("OneOf expander expects at least two choices supplied, but 1 given.", exception.Message);
        }
    }
}
