using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NMatcher.IntegrationTests
{
    public class XmlTests
    {
        [Fact]
        public void it_matches_simple_xml()
        {
            var actual = "<users><user>Foobar</user></users>";
            var expected = "<users><user>Foobar</user></users>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }

        [Fact]
        public void it_matches_xml_with_expression()
        {
            var actual = "<users><user>Foobar</user></users>";
            var expected = "<users><user>@string@</user></users>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }

        [Fact]
        public void it_matches_xml_with_attribute()
        {
            var actual = @"<users><user id=""1"">Foobar</user></users>";
            var expected = @"<users><user id=""@string@"">@string@.Contains('Foo')</user></users>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }
        
        [Fact]
        public void it_matches_more_complex_xml()
        {
            var actual = @"<?xml version=""1.0""?>
                <soap:Envelope xmlns:soap=""http://www.w3.org/2001/12/soap-envelope"" soap:encodingStyle=""http://www.w3.org/2001/12/soap-encoding"">
                <soap:Body xmlns:m=""http://www.example.org/stock"">
                    <m:GetStockPrice>
                        <m:StockName>IBM</m:StockName>
                        <m:StockValue>Any Value</m:StockValue>
                     </m:GetStockPrice>
                </soap:Body>
                </soap:Envelope>";

            var expected = @"<?xml version=""1.0""?>
                <soap:Envelope xmlns:soap = ""http://www.w3.org/2001/12/soap-envelope"" soap:encodingStyle=""http://www.w3.org/2001/12/soap-encoding"">
                <soap:Body xmlns:m=""http://www.example.org/stock"">
                    <m:GetStockPrice>
                        <m:StockName>IBM</m:StockName>
                        <m:StockValue>Any Value</m:StockValue>
                     </m:GetStockPrice>
                </soap:Body>
                </soap:Envelope>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }

        [Fact]
        public void it_matches_more_complex_with_expressions()
        {
            var actual = @"<?xml version=""1.0""?>
                <soap:Envelope xmlns:soap=""http://www.w3.org/2001/12/soap-envelope"" soap:encodingStyle=""http://www.w3.org/2001/12/soap-encoding"">
                <soap:Body xmlns:m=""http://www.example.org/stock"">
                    <m:GetStockPrice>
                        <m:StockName>IBM</m:StockName>
                        <m:StockValue>Any Value</m:StockValue>
                     </m:GetStockPrice>
                </soap:Body>
                </soap:Envelope>";

            var expected = @"<?xml version=""1.0""?>
                <soap:Envelope xmlns:soap=""@string@"" soap:encodingStyle=""@string@"">
                <soap:Body xmlns:m=""http://www.example.org/stock"">
                    <m:GetStockPrice>
                        <m:StockName>@string@</m:StockName>
                        <m:StockValue>@string@.Contains('Any')</m:StockValue>
                     </m:GetStockPrice>
                </soap:Body>
                </soap:Envelope>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }

        [Fact]
        public void it_matches_more_complex_with_expressions_no_2()
        {
            var actual = @"<note>
                <to id=""fuu"" at2=""bar"">Tove</to>
                <heading id=""h2"">
                    <subheader subid=""s2"">fooo</subheader>
                    <mainheader>Reminder</mainheader>
                </heading>
                <body>Don't forget me this weekend!</body>
                </note>";

            var expected = @"<note>
                <to id=""@string@"" at2=""@string@"">Tove</to>
                <from>@string?@</from>
                <heading id=""@string@"">
                    <subheader subid=""@string@"">@string@</subheader>
                    <mainheader>@string@</mainheader>
                </heading>
                <body>@string@</body>
                </note>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }

        [Fact]
        public void it_matches_with_optional()
        {
            var actual = @"<note>
                <to>Tove</to>
                <heading>Reminder</heading>
                <body>Don't forget me this weekend!</body>
                </note>";

            var expected = @"<note>
                <to>Tove</to>
                <from>@string?@</from>
                <heading>Reminder</heading>
                <body>Don't forget me this weekend!</body>
                </note>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }

        [Fact]
        public void it_matches_with_cdata()
        {
            var actual = "<string><![CDATA[Any kind of text here]]></string>";
            var expected = "<string><![CDATA[@string@]]></string>";

            Assert.True(new Matcher().MatchXml(actual, expected));
        }
    }
}
