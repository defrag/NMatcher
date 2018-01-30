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
        public void it_matches_json()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : ""some-uid-here""}", @"{""id"" : ""@string@""}"));
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
                            ""name"" : ""@string@""
                        }
                    }
                }"
            );

            Assert.True(result);
        }

    }
}
