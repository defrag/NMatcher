using Sprache;
using System.Linq;
using Xunit;

namespace NMatcher.Expressions
{
    public class ExpressionParserTests
    {
        [Fact]
        public void test_type()
        {
            var result = ExpressionParser.Type.Parse("@bool@");
            Assert.Equal("bool", result);
        }

        [Fact]
        public void test_expander_without_parameters()
        {
            var result = ExpressionParser.ExpanderWithNoArguments.Parse(".IsGuid()");
            Assert.Equal("IsGuid", result.Name);
        }

        [Fact]
        public void test_quoted_strings()
        {
            var result = ExpressionParser.QuotedString.Parse("\"foooo\"");
            Assert.Equal("foooo", result.Value);
        }

        [Fact]
        public void test_ints()
        {
            var result = ExpressionParser.Integer.Parse("123");
            Assert.Equal(123, result.Value);
        }

        [Fact]
        public void test_double()
        {
            var result = ExpressionParser.Double.Parse("123.12");
            Assert.Equal(123.12, result.Value);
        }

        [Fact]
        public void test_arguments()
        {
            var result = ExpressionParser.Arguments.Parse("1, 11.30, \"string value\"");
            var list = result.ToList();
            Assert.Equal(3, list.Count);
            Assert.Equal(1, list[0].Value);
            Assert.Equal(11.3, list[1].Value);
            Assert.Equal("string value", list[2].Value);

        }

        [Fact]
        public void test_full_expression()
        {
            var result = ExpressionParser.Expression.Parse("@string@.IsGuid()");
        }

        [Fact]
        public void test_multiple_expanders()
        {
            var result = ExpressionParser.Expression.Parse("@string@.IsGuid().Contains(\"42\")");

            Assert.Equal(2, result.Expanders.ToList().Count);
        }

        [Fact]
        public void test_full_expression_with_params()
        {
            var result = ExpressionParser.Expression.Parse("@string@.Contains(\"foo\", 2)");
            Assert.Equal("Contains", result.Expanders.ToList()[0].Name);
        }
    }
}
