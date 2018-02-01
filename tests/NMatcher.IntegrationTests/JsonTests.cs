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
            var result = matcher.MatchJson(@"{""id"" : null}", @"{""id"" : null}");
            Assert.True(result.Successful);
        }

        [Fact]
        public void it_matches_empty_objects()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{}", @"{}"));
        }

        [Fact]
        public void it_matches_with_guid()
        {
            var matcher = new Matcher();

            Assert.True(matcher.MatchJson(@"{""id"" : ""c56a4180-65aa-42ec-a945-5fd21dec0538""}", @"{""id"" : ""@guid@""}"));
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
                    ""uid"": ""C56A4180-65AA-42EC-A945-5FD21DEC0538"", 
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
                            ""_date"" : ""2018-01-01"",
                            ""_endDate"": ""2017-12-01T00:00:00""
                        }
                    }
                }",
                @"
                {
                    ""id"" : ""@string@"",
                    ""uid"": ""@guid@"", 
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
                            ""_date"" : ""@string@.IsDateTime()"",
                            ""_endDate"": ""@string@.IsDateTime()""
                        }
                    }
                }"
            );

            Assert.True(result.Successful);
        }

        [Fact]
        public void it_matches_dates()
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


        [Fact]
        public void it_doesnt_matches_with_keys_that_doesnt_exist()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"{""id"" : 1}", @"{}");
            Assert.False(result.Successful);
            Assert.Equal("Expected value did not appear at path id.", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_matches_with_keys_that_doesnt_exist_in_nested_actual_json()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                {
                    ""id"" : ""some-uid-here"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""80-000"",
                        ""enabled"" : false
                    }
                }",
                @"
                {
                    ""id"" : ""@string@"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""@string@"",
                        ""radius"" : ""1000"",
                        ""enabled"" : ""@bool@""
                    }
                }"
            );

            Assert.False(result.Successful);
            Assert.Equal("Actual value did not appear at path subnode.radius.", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_matches_with_keys_that_doesnt_exist_in_nested_expected_json()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                {
                    ""id"" : ""some-uid-here"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""80-000"",
                        ""enabled"" : false,
                        ""notInExpected"" : true
                    }
                }",
                @"
                {
                    ""id"" : ""@string@"",
                    ""subnode"" : {
                        ""city"" : ""NY"",
                        ""zipCode"" : ""@string@"",
                        ""radius"" : ""1000""
                    }
                }"
            );

            Assert.False(result.Successful);
            Assert.Equal("Expected value did not appear at path subnode.notInExpected.", result.ErrorMessage);
        }
    }
}
