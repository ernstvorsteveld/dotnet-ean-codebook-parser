using System;
using System.Net;
using HtmlAgilityPack;

namespace parser.sternitc.html.parser.util
{
    public class HtmlResult
    {
        private CookieCollection _cookies;
        public HtmlDocument HtmlDocument { get; }

        public HtmlResult(CookieCollection cookies, HtmlDocument htmlDocument)
        {
            _cookies = cookies;
            HtmlDocument = htmlDocument;
        }

        public string GetCookie(string name)
        {
            foreach (Cookie cookie in _cookies)
            {
                if (cookie.Name != name) continue;
                return cookie.Value;
            }

            throw new SessionCookieNotFoundException();
        }
    }

    public class SessionCookieNotFoundException : Exception
    {
    }
}