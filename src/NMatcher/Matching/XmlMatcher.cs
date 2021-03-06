﻿using Newtonsoft.Json;
using System;
#if NETSTANDARD1_3 || NETSTANDARD2_0
using System.Xml.Linq;
#else
using System.Xml;
#endif
namespace NMatcher.Matching
{
    public sealed class XmlMatcher : IMatcher
    {
        private readonly ExpressionMatcher _expressionMatcher;
        private readonly string _expectedXml;

        public XmlMatcher(ExpressionMatcher expressionMatcher, string expectedXml)
        {
            _expressionMatcher = expressionMatcher ?? throw new ArgumentNullException(nameof(expressionMatcher));
            _expectedXml = expectedXml ?? throw new ArgumentNullException(nameof(expectedXml));
        }

        public Result Match(object value)
        {
            return new JsonMatcher(_expressionMatcher, ToJson(_expectedXml)).Match(ToJson(value.ToString()));
        }

        private string ToJson(string xml)
        {
#if NETSTANDARD1_3 || NETSTANDARD2_0
            var doc = XDocument.Parse(xml);
            return JsonConvert.SerializeXNode(doc);
#else
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
#endif
        }
    }
}
