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
        public void it_matches_with_array_of_objects()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(
                @"
                [
                    {""id"": 100, ""enabled"" : true}
                ]", 
                @"
                [
                    {""id"": ""@int@"", ""enabled"" : ""@bool@""}
                ]");

            Assert.True(result.Successful);
        }

        [Fact]
        public void it_matches_with_different_arrays()
        {
            var matcher = new Matcher();
            Assert.False(matcher.MatchJson("[1,2,3,4]", "[1,2,3]"));
            Assert.False(matcher.MatchJson("[1,2,3]", "[1,2,3,4]"));
        }

        [Fact]
        public void it_matches_array_with_expressions()
        {
            var matcher = new Matcher();
            Assert.True(matcher.MatchJson("[1,2]", @"[""@int@"", ""@int@""]"));
        }

        [Fact]
        public void it_matches_with_optional_missing()
        {
            var matcher = new Matcher();
            var res = matcher.MatchJson(@"{""id"" : 1000}", @"{""id"" : ""@int@"", ""city"": ""@string?@"" }");

            Assert.True(res.Successful);
        }

        [Fact]
        public void it_matches_with_optional_not_missing()
        {
            var matcher = new Matcher();
            var res = matcher.MatchJson(@"{""id"" : 1000, ""city"": ""NY""}", @"{""id"" : ""@int@"", ""city"": ""@string?@"" }");

            Assert.True(res.Successful);
        }

        [Fact]
        public void it_doesnt_array_with_expressions_when_it_fails_evaulation()
        {
            var matcher = new Matcher();
            Assert.False(matcher.MatchJson("[1,2]", @"[""@int@"", ""@string@""]"));
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
                        ""state"" : ""enabled"",
                        ""meta"" : {
                            ""name"" : ""fuuuuuu"",
                            ""shipping"": 99.99,
                            ""enabled"" : false,
                            ""_link"" : ""http://example.com?page=2"",
                            ""_something"" : null,
                            ""_arr"" : [1, 2, 3, 4],
                            ""_arr2"" : [""fuu"", ""bar""],
                            ""_date"" : ""2018-01-01"",
                            ""_endDate"": ""2017-12-01T00:00:00"",
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
                        ""state"" : ""@string@.OneOf('enabled', 'disabled')"",
                        ""meta"" : {
                            ""name"" : ""@string@.Contains('fuu')"",
                            ""shipping"": ""@double@"",
                            ""enabled"" : ""@bool@"",
                            ""_link"" : ""@any@"",
                            ""_something"" : ""@null@"",
                            ""_arr"" : [1, 2, 3, 4],
                            ""_arr2"" : ""@array@"",
                            ""_date"" : ""@string@.IsDateTime()"",
                            ""_endDate"": ""@string@.IsDateTime()"",
                            ""_mayNotBeHere"": ""@string?@.Contains('imagine')""
                        }
                    }
                }"
            );

            Assert.True(result.Successful);
        }

        [Fact]
        public void it_matches_with_arrays()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                {
                     ""arr"" : [1, 2, 3, 4]
                }",
                @"
                {
                    ""arr"" : ""@array@""             
                }"
            );

            Assert.True(result.Successful);
        }

        [Fact]
        public void it_matches_with_array_of_objs()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                    [
                        {""id"": 10}
                    ]
                ",
                @"
                    [
                        {""id"": ""@int@""}
                    ],                    
                "
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
        public void it_doesnt_just_values()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"{""id"" : 1}", @"{""id"" : 2}");
            Assert.False(result.Successful);
            Assert.Equal("1 did not match 2 at path id.", result.ErrorMessage);
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
                        ""enabled"" : ""@bool@""
                    }
                }"
            );

            Assert.False(result.Successful);
            Assert.Equal("Expected value did not appear at path subnode.notInExpected.", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_matches_when_nested_values_just_dont_match()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                {
                    ""id"" : ""some-uid-here"",
                    ""subnode"" : {
                        ""city"" : ""NY""
                    }
                }",
                @"
                {
                    ""id"" : ""@string@"",
                    ""subnode"" : {
                        ""city"" : ""LA""
                    }
                }"
            );

            Assert.False(result.Successful);
            Assert.Equal("NY did not match LA at path subnode.city.", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_match_array_with_elements_to_empty_array()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"[{""id"" : 1}]", @"[]");
            Assert.False(result.Successful);
        }

        [Fact]
        public void it_doesnt_match_empty_array_to_array_with_elements()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"[]", @"[{""id"" : 1}]");
            Assert.False(result.Successful);
        }
    }
}
