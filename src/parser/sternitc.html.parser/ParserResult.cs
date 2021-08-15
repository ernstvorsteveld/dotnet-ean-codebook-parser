using System.Collections.Generic;

namespace parser.sternitc.html.parser 
{
    public class ParserResult
    {
        private readonly IDictionary<string, string> _data = new Dictionary<string, string>();

        public string Get(string key)
        {
            _data.TryGetValue(key, out var value);
            return value;
        }

        public void Put(string key, string value)
        {
            _data.Add(key,value);
        }
    }

    public class ParseResultBuilder
    {
        private readonly ParserResult _parserResult;

        public ParseResultBuilder()
        {
            _parserResult = new ParserResult();
        }

        public ParseResultBuilder With(string key, string value)
        {
            _parserResult.Put(key,value);
            return this;
        }

        public ParserResult Build()
        {
            return _parserResult;
        }
    }
}