using System.Collections.Generic;

namespace parser.sternitc.html.parser 
{
    internal interface IParser
    {
        public IEnumerable<ParserResult> Parse(EANSearchCriteria eanSearchCriteria);
    }
}