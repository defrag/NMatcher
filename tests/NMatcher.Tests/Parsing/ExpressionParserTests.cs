using Sprache;
using System.Linq;
using Xunit;

namespace NMatcher.Parsing
{
    public class ExpressionParserTests
    {
        [Fact]
        public void it_parses_type()
        {
            var result = ExpressionParser.Type.Parse("@bool@");
            Assert.Equal("bool", result);
        }

        [Fact]
        public void it_parses_expanders_without_parameters()
        {
            var result = ExpressionParser.ExpanderWithNoArguments.Parse(".IsGuid()");
            Assert.Equal("IsGuid", result.Name);
        }

        [Fact]
        public void it_parses_quoted_string()
        {
            var result = ExpressionParser.DoubleQuotedString.Parse("\"foooo\"");
            Assert.Equal("foooo", result.Value);
        }

        [Fact]
        public void it_parses_single_quoted_string()
        {
            var result = ExpressionParser.SingleQuotedString.Parse("'foooo'");
            Assert.Equal("foooo", result.Value);
        }

        [Fact]
        public void it_parses_ints()
        {
            var result = ExpressionParser.Integer.Parse("123");
            Assert.Equal(123, result.Value);
        }

        [Fact]
        public void it_parses_double()
        {
            var result = ExpressionParser.Double.Parse("123.12");
            Assert.Equal(123.12, result.Value);
        }

        [Fact]
        public void it_parses_argument_list()
        {
            var result = ExpressionParser.Arguments.Parse("1, 11.30, \"string value\", 'string value 2'");
            var list = result.ToList();
            Assert.Equal(4, list.Count);
            Assert.Equal(1, list[0].Value);
            Assert.Equal(11.3, list[1].Value);
            Assert.Equal("string value", list[2].Value);
            Assert.Equal("string value 2", list[3].Value);
        }

        [Fact]
        public void it_parses_full__expression()
        {
            var result = ExpressionParser.Expression.Parse("@string@.IsGuid()");
        }

        [Fact]
        public void it_parses_multiple_expanders()
        {
            var result = ExpressionParser.Expression.Parse("@string@.IsGuid().Contains(\"42\")");

            Assert.Equal(2, result.Expanders.ToList().Count);
        }

        [Fact]
        public void it_parses_full_expression_with_parameters()
        {
            var result = ExpressionParser.Expression.Parse("@string@.Contains(\"foo\", 2)");
            Assert.Equal("Contains", result.Expanders.ToList()[0].Name);
        }
    }
}
