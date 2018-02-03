using Newtonsoft.Json.Linq;
using NMatcher.Json.Pairing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMatcher.Json
{
    internal static class JsonMatcher
    {
        internal static Tuple<IEnumerable<Pair>, IEnumerable<string>, IEnumerable<string>> IterateMatch(string actual, string expected)
        {
            var actualJson = JsonTokenLoader.LoadJson(actual);
            var expectedJson = JsonTokenLoader.LoadJson(expected);
            var actualPaths = JsonTraversal.AccumulatePaths(actualJson).ToList();
            var resolvedPath = new List<string>();
            var pairs = new List<Pair>();

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
                pairs.Add(new Pair(actualValue, expectedValue, token.Path, comparisonResult));

                resolvedPath.AddRange(JsonTraversal.AccumulatePaths(accumulate));
            });

            return Tuple.Create(pairs.AsEnumerable(), actualPaths.AsEnumerable(), resolvedPath.AsEnumerable());
        }         
    }
}
