using parser.sternitc.html.parser;

namespace parser_test.sternitc.html.parser
{
    public abstract class BaseUrlHtmlParserTests
    {
        protected ParserConfiguration BuildConfiguration(string url, string pagingUrl, string domain)
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

        protected (string, string, string) GivenConfiguration()
        {
            const string scheme = "https";
            const string domain = "www.eancodeboek.nl";
            const string url = scheme + "://" + domain + "/eancodeboek/control/index" +
                               "?postcode={0}&huisnummer={1}&marktsegment={2}" +
                               "&bijzondereaansluiting=-&zoekform=true";
            const string pagingUrl = url + "&next=next";
            return (domain, url, pagingUrl);
        }
    }
}