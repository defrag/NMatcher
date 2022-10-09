using NMatcher.Matching.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
        
        private TraversalResult TraverseMatch2(string actualJson)
        {
            var actual = JsonDocument.Parse(actualJson);
            var expected = JsonDocument.Parse(_expectedJson);

            var actualCollected = SystemJsonTraversal.CollectPaths(actual);
            var extectedCollected = SystemJsonTraversal.CollectPaths(expected);

            var actualPaths = actualCollected.Elements.Select(s => s.Path).ToList();
            var resolvedPaths = new List<string>();

            var pairs = new List<JsonPair>();
            foreach (var element in extectedCollected.Elements)
            {
                var expectedNode = extectedCollected.AtPath(element.Path);
                var actualNode = actualCollected.AtPath(element.Path);

                var expectedValue = expectedNode!.ParseValue();
                var actualValue = actualNode?.ParseValue();

                if (actualValue is null)
                {
                    pairs.Add(new JsonPair(null, expectedValue, element.Path, false));
                }
                
                if (expectedValue.ToString().Contains("?"))
                {
                    actualPaths.Add(expectedNode.Path);
                }
                
                resolvedPaths.Add(expectedNode.Path);

                var comparisonResult = false;
                if (actualValue is IEnumerable a && expectedValue is IEnumerable b)
                {
                    

                    comparisonResult = true;
                }
                else
                {
                    comparisonResult = expectedValue == actualValue;
                }
                
                
                pairs.Add(new JsonPair(actualValue, expectedValue, element.Path, comparisonResult));
            }

            
            return new TraversalResult(pairs, resolvedPaths, actualPaths);
        }
        
        public Result Match(object value)
        {
            var result = TraverseMatch2(value.ToString());
            
            var expectedDiff = result.ActualPaths.Except(result.ResolvedPaths);
            if (expectedDiff.Any())
            {
                return Result.Failure($"Expected value did not appear at path {expectedDiff.First()}.");
            }

            var actualDiff = result.ResolvedPaths.Except(result.ActualPaths);
            if (actualDiff.Any())
            {
                return Result.Failure($"Actual value did not appear at path {actualDiff.First()}.");
            }

            return result.Pairs
                   .Select(MatchPair)
                   .FirstOrDefault(_ => false == _.Successful) ?? Result.Success();
        }

        private string FormatError(object act, object exp, string path)
        {
            var ac = act == null ? "null" : act.ToString();
            var ec = exp == null ? "null" : exp.ToString();

            return $"Actual value \"{ac}\" ({act.GetType()}) did not match \"{ec}\" ({exp.GetType()}) at path \"{path}\".";
        }

        private Result MatchPair(JsonPair pair)
        {
            if (pair.Expected != null && ExpressionMatcher.MatcherRegex.IsMatch(pair.Expected.ToString()))
            {
                return _expressionMatcher.MatchExpression(pair.Actual, pair.Expected.ToString());
            }

            return pair.IsEqual ? Result.Success() : Result.Failure(FormatError(pair.Actual, pair.Expected, pair.Path));
        }
        
        private sealed class TraversalResult
        {
            public TraversalResult(IEnumerable<JsonPair> pairs, IEnumerable<string> actualPaths, IEnumerable<string> resolvedPaths)
            {
                Pairs = pairs;
                ActualPaths = actualPaths;
                ResolvedPaths = resolvedPaths;
            }

            public IEnumerable<JsonPair> Pairs { get; }
            public IEnumerable<string> ActualPaths { get; }
            public IEnumerable<string> ResolvedPaths { get; }

        }
    }
}
