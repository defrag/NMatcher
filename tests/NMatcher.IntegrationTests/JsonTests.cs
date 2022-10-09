using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NMatcher.IntegrationTests.XUnit;
using NMatcher.Matching.Json;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class JsonTests
    {
        [Fact]
        public void it_matches_json_with_string()
        {
            JsonAssert.MatchesJson(@"{""id"" : ""some-uid-here""}", @"{""id"" : ""@string@""}");
        }

        [Fact]
        public void it_checks()
        {
            var json = @"
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
                            ""_es"": null
                        }
                    }
                }";
            var paths = SystemJsonTraversal.CollectPaths(JsonDocument.Parse(json));

        }

        
        [Fact]
        public void it_matches_with_int()
        {
            JsonAssert.MatchesJson(@"{""id"" : 1000}", @"{""id"" : ""@int@""}");
        }

        [Fact]
        public void it_matches_with_double()
        {
            JsonAssert.MatchesJson(@"{""price"" : 100.00}", @"{""price"" : ""@double@""}");
        }

        [Fact]
        public void it_matches_with_bool()
        {
            JsonAssert.MatchesJson(@"{""enabled"" : true}", @"{""enabled"" : ""@bool@""}");
        }

        [Fact]
        public void it_matches_with_wildcard()
        {
            JsonAssert.MatchesJson(@"{""id"" : 1000}", @"{""id"" : ""@any@""}");
        }

        [Fact]
        public void it_matches_with_null()
        {
            JsonAssert.MatchesJson(@"{""id"" : null}", @"{""id"" : ""@null@""}");
        }

        [Fact]
        public void it_matches_with_casual_null()
        {
            JsonAssert.MatchesJson(@"{""id"" : null}", @"{""id"" : null}");
        }

        [Fact]
        public void it_matches_empty_objects()
        {
            JsonAssert.MatchesJson(@"{}", @"{}");
        }

        [Fact]
        public void it_matches_with_guid()
        {
            JsonAssert.MatchesJson(@"{""id"" : ""c56a4180-65aa-42ec-a945-5fd21dec0538""}", @"{""id"" : ""@guid@""}");
        }

        [Fact]
        public void it_matches_simple_array()
        {
            JsonAssert.MatchesJson("[1,2,3]", "[1,2,3]");
        }

        [Fact]
        public void it_matches_with_array_of_objects()
        {
            JsonAssert.MatchesJson(
                @"
                [
                    {""id"": 100, ""enabled"" : true}
                ]", 
                @"
                [
                    {""id"": ""@int@"", ""enabled"" : ""@bool@""}
                ]");
        }

        [Fact]
        public void it_matches_with_different_arrays()
        {
            JsonAssert.MatchesJson("[1,2,3,4]", "[1,2,3]");
            JsonAssert.MatchesJson("[1,2,3]", "[1,2,3,4]");
        }

        [Fact]
        public void it_matches_array_with_expressions()
        {
            JsonAssert.MatchesJson("[1,2]", @"[""@int@"", ""@int@""]");
        }

        [Fact]
        public void it_matches_with_optional_missing()
        {
            JsonAssert.MatchesJson(@"{""id"" : 1000}", @"{""id"" : ""@int@"", ""city"": ""@string?@"" }");
        }

        [Fact]
        public void it_matches_with_optional_not_missing()
        {
            JsonAssert.MatchesJson(@"{""id"" : 1000, ""city"": ""NY""}", @"{""id"" : ""@int@"", ""city"": ""@string?@"" }");
        }

        [Fact]
        public void it_doesnt_array_with_expressions_when_it_fails_evaulation()
        {
            JsonAssert.NotMatchesJson("[1,2]", @"[""@int@"", ""@string@""]");
        }
        [Fact]
        public void it_matches_nested_json()
        {

            JsonAssert.MatchesJson(
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
                            ""_link"" : ""http://example.com?page=@int@"",
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

            var result = matcher.MatchJson2(
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
        public void it_doesnt_match_just_values_properly()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson2(@"{""id"" : 1}", @"{""id"" : 2}");
            Assert.False(result.Successful);
            Assert.Equal("Actual value \"1\" (System.Int32) did not match \"2\" (System.Int32) at path \".id\".", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_matches_with_keys_that_doesnt_exist()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson2(@"{""id"" : 1}", @"{}");
            Assert.False(result.Successful);
            Assert.Equal("Expected value did not appear at path id.", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_matches_with_keys_that_doesnt_exist_in_nested_actual_json()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson2(
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

            var result = matcher.MatchJson2(
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

            var result = matcher.MatchJson2(
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
            Assert.Equal("Actual value \"NY\" (System.String) did not match \"LA\" (System.String) at path \".subnode.city\".", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_match_array_with_elements_to_empty_array()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson2(@"[{""id"" : 1}]", @"[]");
            Assert.False(result.Successful);
        }

        [Fact]
        public void it_doesnt_match_empty_array_to_array_with_elements()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson2(@"[]", @"[{""id"" : 1}]");
            Assert.False(result.Successful);
        }

        [Fact]
        public void it_doesnt_match_int_with_double()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson2(@"{""id"" : 1}", @"{ ""id"" : 1.0}");
            Assert.False(result.Successful);
        }

        [Fact]
        public void it_doesnt_match_double_with_int()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson2(@"{""id"" : 2.0}", @"{ ""id"" : 2}");
            Assert.False(result.Successful);
            Assert.Equal("Actual value \"2\" (System.Double) did not match \"2\" (System.Int32) at path \".id\".", result.ErrorMessage);
        }

        [Fact]
        public void it_works_with_empty_array_in_actual_thats_not_matched_in_expected()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson2(
                @"
                 {
                    ""Foo"" : 1,
                    ""SomeArray"": []
                }",
                @"
                {
                    ""Foo"" : 1
                }"
            );

            Assert.False(result.Successful);
            Assert.Equal("Expected value did not appear at path SomeArray.", result.ErrorMessage);
        }

        [Fact]
        public void it_works_with_same_json_for_array_in_object()
        {
            var testJson = @"{""test"" : [1, 2 ,3]}";
            var matcher = new Matcher();
            var result = matcher.MatchJson2(testJson, testJson);
            Assert.True(result.Successful);
        }

        [Fact]
        public void it_works_with_same_json_for_empty_array()
        {
            var testJson = @"{""test"" : []}";
            var matcher = new Matcher();
            var result = matcher.MatchJson2(testJson, testJson);
            Assert.True(result.Successful);
        }
    }
}
