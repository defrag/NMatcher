using Newtonsoft.Json.Linq;
using System;
using static NMatcher.Json.JsonTraversal;

namespace NMatcher.Json
{
    internal static class JsonComparator
    {
        public static void CompareJson(string actual, string expected, Action<object, object, string, Func<bool>> compare)
        {
            var actualJson = JsonTokenLoader.LoadJson(actual);
            var expectedJson = JsonTokenLoader.LoadJson(expected);

            void Compare(JToken node)
            {
                var currentNode = (JValue)actualJson.SelectToken(node.Path);
                var expectedNode = (JValue)expectedJson.SelectToken(node.Path);

                var currentValue = currentNode?.Value;
                var expectedValue = expectedNode?.Value;

                if ((currentValue == null && expectedValue != null) || (currentValue != null && expectedValue == null))
                {
                    compare(currentValue, expectedValue, node.Path, () => false);
                    return;
                }

                compare(currentValue, expectedValue, node.Path, () => currentNode.Equals(expectedNode));
            };

            TraverseJson(expectedJson, Compare);
            TraverseJson(actualJson, Compare);
        }
    }
}
