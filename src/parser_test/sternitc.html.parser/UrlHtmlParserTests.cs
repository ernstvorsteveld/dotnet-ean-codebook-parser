using System.Linq;
using parser.sternitc.html.parser;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace parser_test.sternitc.html.parser
{
    public class UrlHtmlParserTests : BaseUrlHtmlParserTests
    {
        [Theory]
        [InlineData("6412PP", "10", "23", "871688540030933384", "871688540004448586")]
        [InlineData("6412PP", "10", "27", "871688540008568259", "871688540030933391")]
        public void should_load_html_page(string postalcode, string number, string segment, string ean1, string ean2)
        {
            var (scheme, domain, url, pagingUrl) = GivenConfiguration();
            var config = BuildConfiguration(url, pagingUrl, domain);
            var criteria = new EANSearchCriteria(postalcode, number, segment);

            var results = new UrlHtmlParser(config).Parse(criteria).ToList();

            results.Should().HaveCount(2, "because we expect 2 objects.");
            results.Should().SatisfyRespectively(
                first => { first.Get("EAN code aansluiting").Should().Be(ean1); },
                second => { second.Get("EAN code aansluiting").Should().Be(ean2); }
            );
        }

        [Fact]
        public void should_load_html_page_paged()
        {
            var (scheme, domain, url, pagingUrl) = GivenConfiguration();
            var config = BuildConfiguration(url, pagingUrl, domain);
            var criteria = new EANSearchCriteria("6903XE", "1", "23");
            
            var results = new UrlHtmlParser(config).Parse(criteria).ToList();
            results.Count.Should().Be(73);
        }
    }
}