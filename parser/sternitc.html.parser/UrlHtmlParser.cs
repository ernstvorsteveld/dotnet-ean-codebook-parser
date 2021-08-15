using System.Collections.Generic;
using System.Linq;
using parser.sternitc.html.parser.util;

namespace parser.sternitc.html.parser
{
    public class UrlHtmlParser : IParser
    {
        private readonly ParserConfiguration _config;

        public UrlHtmlParser(ParserConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<ParserResult> Parse(EANSearchCriteria criteria)
        {
            var documentLoader = new HtmlDocumentLoader(
                _config.Get(ConfigurationKey.DOMAIN),
                _config.Get(ConfigurationKey.URL),
                _config.Get(ConfigurationKey.PAGING_URL));
            var htmlResult = documentLoader.LoadDocument(criteria, null);
            var attributeNames = GetAttributes(htmlResult).ToList();
            var pageCount = new PageHandler(htmlResult,
                _config.Get(ConfigurationKey.DIV_PAGING)).GetNumberOfPages();
            var result = GetResults(htmlResult, attributeNames).ToList();
            for (var i = 1; i < pageCount; i++)
            {
                htmlResult = documentLoader.LoadDocument(criteria, i.ToString());
                var pageResult = GetResults(htmlResult, attributeNames);
                result.AddRange(pageResult);
            }

            return result;
        }

        private IEnumerable<ParserResult> GetResults(HtmlResult htmlResult, IEnumerable<string> attributeNames)
        {
            var nodes = htmlResult.HtmlDocument.DocumentNode.SelectNodes(_config.Get(ConfigurationKey.TABLE_ROW));
            var td = _config.Get(ConfigurationKey.TABLE_ROW_TD);

            return nodes.Select(current =>
                {
                    var builder = new ParseResultBuilder();
                    var attributeNamesList = attributeNames.ToList();
                    for (int i = 0; i < attributeNamesList.Count; i++)
                    {
                        builder
                            .With(attributeNamesList[i], current.SelectNodes(td)[i].InnerHtml);
                    }

                    return builder.Build();
                })
                .Select(parserResult => parserResult).ToList();
        }

        private IEnumerable<string> GetAttributes(HtmlResult htmlResult)
        {
            var nodes = htmlResult.HtmlDocument.DocumentNode.SelectNodes(_config.Get(ConfigurationKey.TABLE_HEADER));
            return nodes.Select(node => node.InnerHtml).ToList();
        }
    }
}