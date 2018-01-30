using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class JsonTests
    {
        [Fact]
        public void it_matches_json_with_string()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : ""some-uid-here""}", @"{""id"" : ""@string@""}"));
        }

        [Fact]
        public void it_matches_with_int()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : 1000}", @"{""id"" : ""@int@""}"));
        }

        [Fact]
        public void it_matches_with_double()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""price"" : 100.00}", @"{""price"" : ""@double@""}"));
        }

        [Fact]
        public void it_matches_with_bool()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""enabled"" : true}", @"{""enabled"" : ""@bool@""}"));
        }

        [Fact]
        public void it_matches_with_wildcard()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : 1000}", @"{""id"" : ""@any@""}"));
        }

        [Fact]
        public void it_matches_with_null()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : null}", @"{""id"" : ""@null@""}"));
        }

        [Fact]
        public void it_matches_with_casual_null()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : null}", @"{""id"" : null}"));
        }

        [Fact]
        public void it_matches_simple_array()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson("[1,2,3]", "[1,2,3]"));
        }

        [Fact]
        public void it_matches_nested_json()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                {
                    ""id"" : ""some-uid-here"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""80-000"",
                        ""meta"" : {
                            ""name"" : ""fuuuuuu"",
                            ""shipping"": 99.99,
                            ""enabled"" : false,
                            ""_link"" : ""http://example.com?page=2"",
                            ""_something"" : null,
                            ""_arr"" : [1, 2, 3],
                            ""_date"" : ""2018-01-01""
                        }
                    }
                }",
                @"
                {
                    ""id"" : ""@string@"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""@string@"",
                        ""meta"" : {
                            ""name"" : ""@string@.Contains('fuu')"",
                            ""shipping"": ""@double@"",
                            ""enabled"" : ""@bool@"",
                            ""_link"" : ""@any@"",
                            ""_something"" : ""@null@"",
                            ""_arr"" : [1, 2, 3],
                            ""_date"" : ""@string@.IsDateTime()""
                        }
                    }
                }"
            );

            Assert.True(result.Successful);
        }

        [Fact]
        public void it_fails_matchcing_for_nested_json_when_needed()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                {
                    ""id"" : ""some-uid-here"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""80-000"",
                        ""radius"" : ""1000"",
                        ""enabled"" : false
                    }
                }",
                @"
                {
                    ""id"" : ""@string@"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""@int@"",
                        ""radius"" : ""1000"",
                        ""enabled"" : ""@bool@""
                    }
                }"
            );

            Assert.False(result.Successful);
            Assert.Equal("80-000 is not a valid int.", result.ErrorMessage);
        }

    }
}
