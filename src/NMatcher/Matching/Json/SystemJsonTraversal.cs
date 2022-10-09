#nullable enable
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NMatcher.Extensions;

namespace NMatcher.Matching.Json
{
    public record ElementWithPath(JsonElement Element, string Path)
    {
        public object ParseValue()
        {
            return Element.ParseValue();
        }
    }

    public sealed class CollectedElements
    {
        public IReadOnlyCollection<ElementWithPath> Elements { get; }
        private readonly IReadOnlyDictionary<string, ElementWithPath> _dict;

        public CollectedElements(IReadOnlyCollection<ElementWithPath> elements)
        {
            Elements = elements;
            _dict = elements.ToDictionary(e => e.Path, e2 => e2);
        }

        public ElementWithPath? AtPath(string path) => 
            _dict.ContainsKey(path) ? _dict[path] : null;
    }
    
    public static class SystemJsonTraversal
    {
        public static CollectedElements CollectPaths(JsonDocument doc)
        {
            var elems = new List<ElementWithPath>();

            void Iter(JsonElement node, string origin)
            {
                if (node.ValueKind == JsonValueKind.Array)
                {
                    elems.Add(new ElementWithPath(node, origin));
                    var all = node.EnumerateArray().ToArray();
                    for (int i = 0; i < all.Length; i++)
                    {

                        Iter(all[i], $"{origin}[{i}]");
                    }
                }
                else if (node.ValueKind == JsonValueKind.Object)
                {
                    var all = node.EnumerateObject().ToArray();
                    
                    foreach (var child in all)
                    {
                        var separator = origin.EndsWith('.') ? "" : ".";
                        Iter(child.Value, $"{origin}{separator}{child.Name}");
                    }
                }
                
                else if (origin != "")
                {
                    elems.Add(new ElementWithPath(node, origin));
                }
            }
            
            Iter(doc.RootElement, "");

            return new CollectedElements(elems);
        }
        
        internal static void TraverseAllPaths(JsonElement node, Action<JsonElement> action)
        {
            if (node.ValueKind == JsonValueKind.Array)
            {
                var all = node.EnumerateArray().ToArray();

                if (all.Length == 0)
                {
                    action(node);
                }

                foreach (var child in all)
                {
                    action(child);
                }
            }
            else if (node.ValueKind == JsonValueKind.Object)
            {
                var all = node.EnumerateObject().ToArray();
                    
                foreach (var child in all)
                {
                    TraverseAllPaths(child.Value, action);
                }
            }
        
            action(node);
        }
    }
}