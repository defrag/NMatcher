using Newtonsoft.Json.Linq;
using NMatcher.Json.Pairing.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NMatcher.Json.JsonTraversal;

namespace NMatcher.Json.Pairing
{
    internal static class JsonPairing
    {
        public static List<Pair> PairJson(string actual, string expected)
        {
            var actualJson = JsonTokenLoader.LoadJson(actual);
            var expectedJson = JsonTokenLoader.LoadJson(expected);

            EnsurePathsMatch(actualJson, expectedJson);

            var pairs = new List<Pair>();

            void Pair(JToken node)
            {
                var currentNode = (JValue)actualJson.SelectToken(node.Path);
                var expectedNode = (JValue)expectedJson.SelectToken(node.Path);

                var currentValue = currentNode?.Value;
                var expectedValue = expectedNode?.Value;

                pairs.Add(new Pair(currentValue, expectedValue, node.Path, currentNode.Equals(expectedNode)));
            };

            TraverseJson(expectedJson, Pair);

            return pairs;
        }

        private static void EnsurePathsMatch(JToken actualJson, JToken expectedJson)
        {
            var actualPaths = AccumulatePaths(actualJson);
            var expectedPaths = AccumulatePaths(expectedJson);

            var expectedDiff = actualPaths.Except(expectedPaths);
            if (expectedDiff.Any())
            {
                throw new PathMissingException($"Expected value did not appear at path {expectedDiff.First()}.");
            }

            var actualDiff = expectedPaths.Except(actualPaths);
            if (actualDiff.Any())
            {
                throw new PathMissingException($"Actual value did not appear at path {actualDiff.First()}.");
            }
        }
    }
}
