using System;
using System.Collections;
using System.Collections.Generic;

namespace parser.sternitc.html.parser
{
    public enum ConfigurationKey
    {
        URL,
        PAGING_URL,
        TABLE_HEADER,
        TABLE_ROW,
        TABLE_ROW_TD,
        DIV_PAGING,
        DOMAIN
    }
    public class ParserConfiguration
    {
        private readonly IDictionary<ConfigurationKey, string> _configuration = new Dictionary<ConfigurationKey, string>();

        internal void Set(ConfigurationKey key, string value)
        {
            _configuration.Add(key,value);
        }

        public string Get(ConfigurationKey key)
        {
            _configuration.TryGetValue(key, out var missing);
            return missing;
        }
    }

    public class ParserConfigurationBuilder
    {
        private readonly ParserConfiguration _parserConfiguration;

        public ParserConfigurationBuilder()
        {
            _parserConfiguration = new ParserConfiguration();
        }
        
        public ParserConfigurationBuilder WithConfig(ConfigurationKey key, string value)
        {
            _parserConfiguration.Set(key, value);
            return this;
        }

        public ParserConfiguration Build()
        {
            return _parserConfiguration;
        }
    }
}