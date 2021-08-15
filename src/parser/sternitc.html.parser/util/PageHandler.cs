using System.Collections.Generic;
using HtmlAgilityPack;

namespace parser.sternitc.html.parser.util
{
    public class PageHandler
    {
        private readonly HtmlDocument _htmlDoc;
        private readonly string _pagingXPath;

        public PageHandler(HtmlResult htmlResult, string pagingXPath)
        {
            _htmlDoc = htmlResult.HtmlDocument;
            _pagingXPath = pagingXPath;
        }

        public int GetNumberOfPages()
        {
            var part = _htmlDoc
                .DocumentNode
                .SelectNodes(_pagingXPath);
            if (part == null) return 1;

            foreach (var element in part.Elements())
            {
                var content = element.InnerHtml;
                if (content.Contains(PageMarker))
                {
                    return GetNumberOfPages(content);
                }
            }

            return 1;
        }

        private int GetNumberOfPages(string content)
        {
            if (content == null) return 1;

            var parts = content.Split(Separator);
            var countAsString = FindPageMarker(parts);
            return int.Parse(countAsString.TrimStart().TrimEnd());
        }

        private string FindPageMarker(IEnumerable<string> parts)
        {
            foreach (var part in parts)
            {
                if (!part.Contains(PageMarker)) continue;

                var clean = part.TrimStart();
                return clean[(PageMarker.Length)..];
            }

            return "1";
        }

        private static string Separator => "\n";
        private static string PageMarker => "Pagina 1 van";
    }
}