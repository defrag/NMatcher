#nullable enable
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using NMatcher.Extensions;

namespace NMatcher.Matching.Json
{
    public record ElementWithPath(JsonElement Element, string Path, string? ParentPath = null)
    {
        public object? ParseValue()
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
            
        public IReadOnlyCollection<string> PathsWithSameParent(ElementWithPath e)
            => Elements.Where(p => p.ParentPath == e.ParentPath).Select(p => p.Path).ToList();

        public IReadOnlyCollection<string> DescendantPathsOf(ElementWithPath e)
            => Elements.Select(s => s.Path).Where(p => p.StartsWith(e.Path)).ToList();
    }
    
    public static class SystemJsonTraversal
    {
        public static CollectedElements CollectPaths(JsonDocument doc)
        {
            var elems = new List<ElementWithPath>();

            void Iter(JsonElement node, string origin, string? parentPath = null)
            {
                if (node.ValueKind == JsonValueKind.Array)
                {
                    elems.Add(new ElementWithPath(node, origin, parentPath));
                    var all = node.EnumerateArray().ToArray();
                    for (int i = 0; i < all.Length; i++)
                    {

                        Iter(all[i], $"{origin}[{i}]", origin);
                    }
                }
                else if (node.ValueKind == JsonValueKind.Object)
                {
                    var all = node.EnumerateObject().ToArray();
                    
                    foreach (var child in all)
                    {
                        var separator = origin.EndsWith('.') ? "" : ".";
                        Iter(child.Value, $"{origin}{separator}{child.Name}", origin);
                    }
                }
                
                else if (origin != "")
                {
                    elems.Add(new ElementWithPath(node, origin, parentPath));
                }
            }
            
            Iter(doc.RootElement, "");

            return new CollectedElements(elems);
        }
    }
}