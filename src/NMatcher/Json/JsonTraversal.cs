using Newtonsoft.Json.Linq;
using System;

namespace NMatcher.Json
{
    public static class JsonTraversal
    {
        public static void TraverseJson(JToken node, Action<JToken> action)
        {
            if (node.Type == JTokenType.Object)
            {
                foreach (JProperty child in node.Children<JProperty>())
                {
                    TraverseJson(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    TraverseJson(child, action);
                }
            }
            else
            {
                action((JToken)node);
            }
        }
    }
}
