namespace parser.sternitc.html.parser
{
    public class EANSearchCriteria
    {
        private readonly string _postalCode;
        private readonly string _number;
        private readonly string _segment;

        public EANSearchCriteria(string postalCode, string number, string segment)
        {
            _postalCode = postalCode;
            _number = number;
            _segment = segment;
        }

        public string GetPostCode()
        {
            return _postalCode;
        }

        public string GetNumber()
        {
            return _number;
        }

        public string GetSegment()
        {
            return _segment;
        }
    }
}