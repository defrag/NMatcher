using Newtonsoft.Json.Linq;
using NMatcher.Matching.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NMatcher.Matching
{
    public sealed class JsonMatcher : IMatcher
    {
        private readonly ExpressionMatcher _expressionMatcher;
        private readonly string _expectedJson;

        public JsonMatcher(ExpressionMatcher expressionMatcher, string expectedJson)
        {
            _expressionMatcher = expressionMatcher ?? throw new ArgumentNullException(nameof(expressionMatcher));
            _expectedJson = expectedJson ?? throw new ArgumentNullException(nameof(expectedJson));
        }

        public Result Match(object value)
        {
            var result = IterateMatch(value.ToString(), _expectedJson);

            var expectedDiff = result.Item2.Except(result.Item3);
            if (expectedDiff.Any())
            {
                return Result.Failure($"Expected value did not appear at path {expectedDiff.First()}.");
            }

            var actualDiff = result.Item3.Except(result.Item2);
            if (actualDiff.Any())
            {
                return Result.Failure($"Actual value did not appear at path {actualDiff.First()}.");
            }

            var error = result.Item1
                   .Select(ToPairingResult)
                   .FirstOrDefault(_ => false == _.Successful);

            return error ?? Result.Success();
        }

        private string FormatError(object act, object exp, string path)
        {
            var ac = act == null ? "null" : act.ToString();
            var ec = exp == null ? "null" : exp.ToString();

            return $"{ac} did not match {ec} at path {path}.";
        }

        private Result ToPairingResult(JsonPair pair)
        {
            if (pair.Expected != null && ExpressionMatcher.MatcherRegex.IsMatch(pair.Expected.ToString()))
            {
                return _expressionMatcher.MatchExpression(pair.Actual, pair.Expected.ToString());
            }

            return pair.IsEqual ? Result.Success() : Result.Failure(FormatError(pair.Actual, pair.Expected, pair.Path));
        }

        private Tuple<IEnumerable<JsonPair>, IEnumerable<string>, IEnumerable<string>> IterateMatch(string actual, string expected)
        {
            var actualJson = JsonTokenLoader.LoadJson(actual);
            var expectedJson = JsonTokenLoader.LoadJson(expected);
            var actualPaths = JsonTraversal.AccumulatePaths(actualJson).ToList();
            var resolvedPath = new List<string>();
            var pairs = new List<JsonPair>();

            JsonTraversal.TraverseJson(expectedJson, token =>
            {
                var expectedNode = expectedJson.SelectToken(token.Path);
                var currentNode = actualJson.SelectToken(token.Path);

                object actualValue = null;
                var accumulate = token;
                if (null != currentNode)
                {
                    actualValue = currentNode.Type == JTokenType.Array
                       ? currentNode.Children()
                           .Select(_ => (JValue)_)
                           .Select(_ => _.Value)
                           .ToArray()
                       : ((JValue)currentNode)?.Value;
                    
                    if (currentNode.Type == JTokenType.Array)
                    {
                        accumulate = currentNode;
                    }
                }
                
                if (expectedNode.ToString().Contains("?"))
                {
                    actualPaths.Add(expectedNode.Path);
                }
                
                var expectedValue = ((JValue)expectedNode)?.Value;
                var comparisonResult = currentNode != null ? currentNode.Equals(expectedNode) : false;
                pairs.Add(new JsonPair(actualValue, expectedValue, token.Path, comparisonResult));

                resolvedPath.AddRange(JsonTraversal.AccumulatePaths(accumulate));
            });

            return Tuple.Create(pairs.AsEnumerable(), actualPaths.AsEnumerable(), resolvedPath.AsEnumerable());
        }
    }
}
