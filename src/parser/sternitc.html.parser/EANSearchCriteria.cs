using System;

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

    public class SearchCriteriaBuilder
    {
        private string _postalCode;
        private string _number;
        private string _segment;

        public SearchCriteriaBuilder PostalCode(string postalcode)
        {
            _postalCode = postalcode;
            return this;
        }

        public SearchCriteriaBuilder Number(string number)
        {
            _number = number;
            return this;
        }

        public SearchCriteriaBuilder Segment(string segment)
        {
            _segment = segment;
            return this;
        }

        public EANSearchCriteria Build()
        {
            return new EANSearchCriteria(_postalCode, _number, _segment);
        }
    }
}