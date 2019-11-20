namespace Cyxor.Models
{
    public enum FilterComparison
    {
        Equal,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        InRange,
        NotEqual,

        Contains,
        StartsWith,
        EndsWith
    }

    public enum FilterOperator
    {
        And,
        Or,
    }

    public class FilterApiModel
    {
        public string Property { get; set; }
        public object Value { get; set; }
        public object ValueRange { get; set; }
        public FilterOperator Operator { get; set; }
        public FilterComparison Comparison { get; set; }
    }
}
