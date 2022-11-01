using Xunit;

namespace NMatcher.Matching2
{
    public class Matcher2Tests
    {
        [Fact]
        public void test_matching_ints()
        {
            var i = new Int(Int.GreaterThan(0), Int.LowerThan(5));
            var rs = i.Match(new DynamicValue(4, DynamicValueKind.Int));
            Assert.True(rs.Successful, rs.ErrorMessage);
        }
        
        [Fact]
        public void test_matching_optional_ints()
        {
            var i = new Optional(new Int(Int.GreaterThan(0), Int.LowerThan(5)));
            var rs = i.Match(new DynamicValue(null, DynamicValueKind.Null));
            Assert.True(rs.Successful, rs.ErrorMessage);
        }

        [Fact]
        public void test_matching_strings()
        {
            var i = new String(String.Contains("fuu"), String.OneOf("fuu", "bfuu"));
            var rs = i.Match(new DynamicValue("fuu", DynamicValueKind.String));
            Assert.True(rs.Successful, rs.ErrorMessage);
        }
        
        [Fact]
        public void test_matching_string_datetime()
        {
            var i = new String(String.IsDateTime());
            var rs = i.Match(new DynamicValue("2022-01-01", DynamicValueKind.String));
            Assert.True(rs.Successful, rs.ErrorMessage);
        }
    }    
}

