using NMatcher.Matching.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NMatcher.Matching
{
    public sealed class JsonMatcher : IMatcher
    {
        private readonly ExpressionMatcher _expressionMatcher;
        private readonly string _expectedJson;
        private static readonly string SkipPattern =  "@skip@";
        public JsonMatcher(ExpressionMatcher expressionMatcher, string expectedJson)
        {
            _expressionMatcher = expressionMatcher ?? throw new ArgumentNullException(nameof(expressionMatcher));
            _expectedJson = expectedJson ?? throw new ArgumentNullException(nameof(expectedJson));
        }
        public Result Match(DynamicValue value)
        {
            var result = TraverseMatch(value.Value.ToString());
            
            var expectedDiff = result.ActualPaths.Except(result.ResolvedPaths).ToList();
            if (expectedDiff.Any())
            {
                var first = expectedDiff.First();
                return Result.Failure($"Missing path in actual JSON detected! Expected path \"{first}\" did not appear in actual JSON.");
            }

            var actualDiff = result.ResolvedPaths.Except(result.ActualPaths).ToList();
            if (actualDiff.Any())
            {
                var first = actualDiff.First();
                return Result.Failure($"Extra path in actual JSON detected! Expected path did not include path \"{first}\", which appeared in actual JSON.");
            }

            return result.Pairs
                .Select(ReformatPair)
                .FirstOrDefault(_ => false == _.Successful) ?? Result.Success();
        }
        
        private TraversalResult TraverseMatch(string actualJson)
        {
            var options = new JsonDocumentOptions()
            {
                AllowTrailingCommas = true
            };
            
            using var actualJ = JsonDocument.Parse(actualJson, options);
            using var expectedJ = JsonDocument.Parse(_expectedJson, options);
            var actualCollected = JsonTraversal.CollectPaths(actualJ);
            var expectedCollected = JsonTraversal.CollectPaths(expectedJ);
            
            var actualAcc = actualCollected.AllPaths.ToList();
            var expectedResolvedPaths = new List<string>();
            
            var pairs = new List<JsonPair>();
            foreach (var element in expectedCollected.Elements)
            {
                var expectedNode = expectedCollected.AtPath(element.Path);
                var actualNode = actualCollected.AtPath(element.Path);
                
                var expectedValue = expectedNode?.ParseValue();
                var actualValue = actualNode?.ParseValue();

                expectedResolvedPaths.Add(expectedNode!.Path);
                
                if (expectedNode.Element.ValueKind == JsonValueKind.Array && actualNode?.Element.ValueKind == JsonValueKind.Array)
                {
                    continue;
                }

                var expectedStr = (expectedValue?.ToString() ?? string.Empty);
                
                if (expectedStr == SkipPattern)
                {
                    expectedResolvedPaths.AddRange(
                        actualCollected.PathsDownstreamFromParent(element)
                    );
                    
                    actualAcc.Add(element.Path);
                    continue;
                }
                
                if (ExpressionMatcher.MatcherRegex.IsMatch(expectedStr))
                {
                    var result = _expressionMatcher.MatchExpression(actualValue, expectedStr);
                    pairs.Add(new JsonPair(actualValue, expectedValue, element.Path, result.Successful, JsonPair.ComparisonOrigin.Expression));
                    
                    expectedResolvedPaths.AddRange(
                        actualCollected.DescendantPathsOf(element)
                    );
                    actualAcc.Add(element.Path);
                    continue;
                }
                
                if (actualValue is null)
                {
                    pairs.Add(new JsonPair(null, expectedValue, element.Path, expectedValue == actualValue));
                    continue;
                }
                
                pairs.Add(new JsonPair(actualValue, expectedValue, element.Path, expectedValue is not null && expectedValue.Equals(actualValue)));
            }
            
            return new TraversalResult(pairs, expectedResolvedPaths, actualAcc);
        }
        
        private string FormatError(JsonPair pair)
        {
            return $"Actual value \"{pair.ActualAsString}\" ({pair.ActualType}) did not match \"{pair.ExpectedAsString}\" ({pair.ExpectedType}) at path \"{pair.Path}\".";
        }

        private Result ReformatPair(JsonPair pair)
        {
            return pair.IsEqual 
                ? Result.Success() 
                : Result.Failure(FormatError(pair));
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
