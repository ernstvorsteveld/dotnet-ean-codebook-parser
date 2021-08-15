using System.Linq;
using parser.sternitc.html.parser;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace parser_test.sternitc.html.parser
{
    public class UrlHtmlParserTests
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

        private static ParserConfiguration BuildConfiguration(string url, string pagingUrl, string domain)
        {
            return new ParserConfigurationBuilder()
                .WithConfig(ConfigurationKey.URL, url)
                .WithConfig(ConfigurationKey.PAGING_URL, pagingUrl)
                .WithConfig(ConfigurationKey.DOMAIN, domain)
                .WithConfig(ConfigurationKey.TABLE_HEADER, ".//th[@class='resultHeader']")
                .WithConfig(ConfigurationKey.TABLE_ROW, ".//tbody/tr")
                .WithConfig(ConfigurationKey.TABLE_ROW_TD, "td")
                .WithConfig(ConfigurationKey.DIV_PAGING, ".//div[@id='paginering']")
                .Build();
        }

        private (string, string, string, string) GivenConfiguration()
        {
            const string scheme = "https";
            const string domain = "www.eancodeboek.nl";
            const string url = scheme + "://" + domain + "/eancodeboek/control/index" +
                               "?postcode={0}&huisnummer={1}&marktsegment={2}" +
                               "&bijzondereaansluiting=-&zoekform=true";
            const string pagingUrl = url + "&next=next";
            return (scheme, domain, url, pagingUrl);
        }
    }
}