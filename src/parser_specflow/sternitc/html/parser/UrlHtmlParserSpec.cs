using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using parser_test.sternitc.html.parser;
using parser.sternitc.html.parser;
using TechTalk.SpecFlow;

namespace parser_specflow.sternitc.html.parser
{
    [Binding]
    public class UrlHtmlParserSpec : BaseUrlHtmlParserTests
    {
        private SearchCriteriaBuilder _builder = new SearchCriteriaBuilder();
        private string _domain;
        private string _url;
        private string _pagingUrl;
        private ParserConfiguration _config;
        private IEnumerable<ParserResult> _results;

        [Given(@"a configured parser")]
        public void GivenAConfiguredParser()
        {
            (_domain, _url, _pagingUrl) = GivenConfiguration();
            _config = BuildConfiguration(_url, _pagingUrl, _domain);
        }

        [Given(@"the postalcode is (.*)")]
        public void GivenThePostalcodeIs(string postalcode)
        {
            _builder.PostalCode(postalcode);
        }

        [Given(@"the house number is (.*)")]
        public void GivenTheHousnumberIs(string number)
        {
            _builder.Number(number);
        }

        [Given(@"the product type is (.*)")]
        public void GivenTheProductTypeIsGas(string type)
        {
            _builder.Segment(type == "gas" ? "23" : "27");
        }

        [When(@"retrieve the ean code")]
        public void WhenRetrieveTheEanCode()
        {
            _results = new UrlHtmlParser(_config).Parse(_builder.Build()).ToList();
        }

        [Then(@"ean1 should be (.*) and ean2 should be (.*)")]
        public void ThenEanShouldBeAndEanShouldBe(string ean1, string ean2)
        {
            _results.First().Get("EAN code aansluiting").Should().Be(ean1);
            _results.Last().Get("EAN code aansluiting").Should().Be(ean2);
        }
    }
}