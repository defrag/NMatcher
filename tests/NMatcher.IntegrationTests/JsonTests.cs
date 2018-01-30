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
                            ""name"" : ""fuuuuuu""
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
                            ""name"" : ""@string@.Contains('fuu')""
                        }
                    }
                }"
            );

            Assert.True(result);
        }

    }
}
