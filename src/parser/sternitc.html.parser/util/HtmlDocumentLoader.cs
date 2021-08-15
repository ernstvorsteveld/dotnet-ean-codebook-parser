#nullable enable
using System;
using System.Net;
using HtmlAgilityPack;
using static System.String;

namespace parser.sternitc.html.parser.util
{
    public class HtmlDocumentLoader
    {
        private readonly string _domain;
        private readonly bool _isFile;
        private readonly string _url;
        private readonly string _pagedUrl;
        private CookieCollection _cookies = null!;
        private CookieContainer _cookieContainer = null!;

        public HtmlDocumentLoader(string domain, string uri, string pagedUri)
        {
            _domain = domain;
            if (uri.StartsWith(Http))
            {
                _url = uri;
                _pagedUrl = pagedUri;
                _isFile = false;
            }
            else if (uri.StartsWith(File))
            {
                _url = uri[(FilePrefix.Length - 1)..];
                _pagedUrl = pagedUri;
                _isFile = true;
            }
            else
            {
                throw new UrlNotRecognizedException();
            }
        }

        public HtmlResult LoadDocument(EANSearchCriteria criteria, string pageNumber)
        {
            if (_isFile)
            {
                var doc = new HtmlDocument();
                doc.Load(_url + pageNumber);
                return new HtmlResult(null, doc);
            }
            else
            {
                var web = new HtmlWeb
                {
                    UseCookies = true,
                    PostResponse = PostResponseHandler,
                    PreRequest = PreRequestHandler
                };
                var result = web.Load(GetUrl(criteria, pageNumber));
                return new HtmlResult(_cookies, result);
            }
        }

        private bool PreRequestHandler(HttpWebRequest request)
        {
            request.CookieContainer = _cookieContainer;
            return true;
        }

        private string GetUrl(EANSearchCriteria criteria, string pageNumber)
        {
            var isFirstPage = pageNumber == null;
            return Format(
                isFirstPage ? _url : _pagedUrl,
                criteria.GetPostCode(),
                criteria.GetNumber(),
                criteria.GetSegment(),
                isFirstPage ? "" : Next);
        }

        private void PostResponseHandler(HttpWebRequest request, HttpWebResponse response)
        {
            var sessionHeaderValue = response.Headers.Get(SetCookie);
            if (sessionHeaderValue != null)
            {
                var (value, path) = GetCookieDetails(sessionHeaderValue);
                var cookie = new Cookie(Jsessionid, value)
                {
                    Domain = _domain,
                    Path = path
                };
                var collection = new CookieCollection {cookie};
                ManageCookies(collection);
            }
            else
            {
                if (response.Cookies.Count > 0)
                {
                    ManageCookies(response.Cookies);
                }
            }
        }

        private (string?, string?) GetCookieDetails(string? sessionHeaderValue)
        {
            if (sessionHeaderValue == null)
            {
                return (null, null);
            }

            var parts = sessionHeaderValue.Split(SemiColumn);
            var sessionIdParts = parts[0].Split(EqualsSign);
            var value = sessionIdParts[1];
            var domainParts = parts[1].Split(EqualsSign);
            var domain = domainParts[1];
            return (value, domain);
        }

        private void ManageCookies(CookieCollection collection)
        {
            _cookies ??= new CookieCollection();
            _cookies.Add(collection);
            _cookieContainer = new CookieContainer();
            _cookieContainer.Add(_cookies);
        }

        private static string Http => "http";
        private static string File => "file://";
        private static string Jsessionid => "JSESSIONID";
        private static string EqualsSign => "=";
        private static string SemiColumn => ";";
        private static string SetCookie => "Set-Cookie";
        private static string Next => "next";
        private static string FilePrefix => "file://";
    }

    public class UrlNotRecognizedException : Exception
    {
        public UrlNotRecognizedException() : base("Url Not recognized.")
        {
        }
    }
}