using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMatcher.Matching.Json
{
    public static class JsonTraversal
    {
        public static void TraverseJson(JToken node, Action<JToken> action)
        {
            TraverseAllPaths(node, t =>
            {
                if (t.Type != JTokenType.Array)
                {
                    action(t);
                }
            });
        }

        internal static IEnumerable<string> AccumulatePaths(JToken token)
        {
            var res = new List<string>();
            TraverseAllPaths(token, node =>
            {
                res.Add(node.Path);
            });

            return res;
        }

        internal static void TraverseAllPaths(JToken node, Action<JToken> action)
        {
            if (node.Type == JTokenType.Object)
            {
                foreach (JProperty child in node.Children<JProperty>())
                {
                    TraverseAllPaths(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                var all = node.Children();

                if (all.Count() == 0)
                {
                    action(node);
                }

                foreach (JToken child in all)
                {
                    TraverseAllPaths(child, action);
                }
            }
            else
            {
                action(node);
            }
        }
    }
}
