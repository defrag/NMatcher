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
            JsonAssert.NotMatchesJson("[1,2,3,4]", "[1,2,3]");
            JsonAssert.NotMatchesJson("[1,2,3]", "[1,2,3,4]");
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
                            ""_endDate"": ""2017-12-01T00:00:00""
                        }
                    },
                    ""subnodeArr"": [1, 2, 3, 4,5,6] 
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
                    },
                    ""subnodeArr"": [1, 2, 3, ""@skip@""]
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
                    ]                   
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
            Assert.Equal("Actual value \"80-000\" (System.String) did not match \"@int@\" (Expression) at path \".subnode.zipCode\".", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_match_just_values_properly()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"{""id"" : 1}", @"{""id"" : 2}");
            Assert.False(result.Successful);
            Assert.Equal("Actual value \"1\" (System.Int32) did not match \"2\" (System.Int32) at path \".id\".", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_matches_with_keys_that_doesnt_exist()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"{""id"" : 1}", @"{}");
            Assert.False(result.Successful);
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
            Assert.Equal("Missing path in actual JSON detected! Expected path \".subnode.radius\" did not appear in actual JSON.", result.ErrorMessage);
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
            Assert.Equal("Extra path in actual JSON detected! Expected path did not include path \".subnode.notInExpected\", which appeared in actual JSON.", result.ErrorMessage);
        }

        [Fact]
        public void it_doesnt_match_when_nested_values_just_dont_match()
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
            Assert.Equal("Actual value \"NY\" (System.String) did not match \"LA\" (System.String) at path \".subnode.city\".", result.ErrorMessage);
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

        [Fact]
        public void it_doesnt_match_int_with_double()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"{""id"" : 1}", @"{ ""id"" : 1.0}");
            Assert.False(result.Successful);
        }

        [Fact]
        public void it_doesnt_match_double_with_int()
        {
            var matcher = new Matcher();
            var result = matcher.MatchJson(@"{""id"" : 2.0}", @"{ ""id"" : 2}");
            Assert.False(result.Successful);
            Assert.Equal("Actual value \"2\" (System.Double) did not match \"2\" (System.Int32) at path \".id\".", result.ErrorMessage);
        }

        [Fact]
        public void it_works_with_empty_array_in_actual_thats_not_matched_in_expected()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
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
            Assert.Equal("Extra path in actual JSON detected! Expected path did not include path \".SomeArray\", which appeared in actual JSON.", result.ErrorMessage);
        }

        [Fact]
        public void it_works_with_same_json_for_array_in_object()
        {
            var testJson = @"{""test"" : [1, 2 ,3]}";
            var matcher = new Matcher();
            var result = matcher.MatchJson(testJson, testJson);
            Assert.True(result.Successful);
        }

        [Fact]
        public void it_works_with_same_json_for_empty_array()
        {
            var testJson = @"{""test"" : []}";
            var matcher = new Matcher();
            var result = matcher.MatchJson(testJson, testJson);
            Assert.True(result.Successful);
        }
        
        [Fact]
        public void it_prevents_expressions_with_typo_from_build_evaluated()
        {
            JsonAssert.NotMatchesJson(@"{""id"" : ""7cc7e59a-4dcf-4cdb-83ae-326bf81c78da""}", @"{""id"" : ""@guid@a""}");
        }
        
        [Fact]
        public void it_matches_array_with_skip()
        {
            JsonAssert.MatchesJson(
                @"[""foo"", ""bar"", ""baz""]",
                @"[""foo"", ""@skip@""]"
            );
        }
        
        [Fact]
        public void it_matches_array_object_with_skip()
        {
            JsonAssert.MatchesJson(
                @"
                {
                     ""arr"" : [1, 2, 3, 4]
                }",
                @"
                {
                    ""arr"" : [1, 2, ""@skip@""]            
                }"
            );
        }
        
        [Fact]
        public void it_doesnt_match_with_skip_if_values_were_incorrect()
        {
            JsonAssert.NotMatchesJson(
                @"
                {
                     ""arr"" : [1, 2, 3, 4]
                }",
                @"
                {
                    ""arr"" : [3, 2, ""@skip@""]            
                }"
            );
        }
        
        [Fact]
        public void it_matches_with_nested_skips()
        {
            JsonAssert.MatchesJson(
                @"
                {
                     ""arr"" : [1, 2, 3, 4],
                     ""foo"": {
                        ""bar"" : [""milk"", ""flour"", ""sugar""]
                     }
                }",
                @"
                {
                    ""arr"" : [1, 2, ""@skip@""],
                    ""foo"": {
                        ""bar"" : [""milk"", ""@skip@""]
                     }
                }"
            );
        }
        
        [Fact]
        public void it_matches_with_skip_and_array_of_objs()
        {
            JsonAssert.MatchesJson(
                @"
                    [
                        {""name"": ""Michal""}, 
                        {""name"": ""Johny""}
                    ]
                ",
                @"
                    [
                        {""name"": ""Michal""}, 
                        ""@skip@""
                    ]      
                "
            );
        }
        
        [Fact]
        public void it_works_with_trailing_commas()
        {
            var matcher = new Matcher();

            var result = matcher.MatchJson(
                @"
                    [
                        {""id"": 10,}
                    ]
                ",
                @"
                    [
                        {""id"": ""@int@""}
                    ]                   
                "
            );

            Assert.True(result.Successful);
        }
        
        [Fact]
        public void it_matches_with_skip_and_array_of_complex_objs()
        {
            JsonAssert.MatchesJson(
                @"{
                    ""users"":
                    [
                        {""name"": ""Michal"", ""id"": 1, ""bar"": {""baz"" : ""kaz"", ""zaz"": [1,2,3] }}, 
                        {""name"": ""Johny"",  ""id"": 2, ""bar"": {""baz"" : ""kaz"", ""zaz"": [1,2,3] }}
                    ]
                }",
                @"{
                    ""users"":
                    [
                        {""name"": ""Michal"", ""id"": 1, ""bar"": {""baz"" : ""kaz"", ""zaz"": [1,2,3] }}, 
                        ""@skip@""
                    ]      
                }"
            );
        }

    }
}
