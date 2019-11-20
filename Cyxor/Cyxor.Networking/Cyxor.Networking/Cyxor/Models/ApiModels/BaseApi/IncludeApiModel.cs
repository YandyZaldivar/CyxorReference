using System.ComponentModel;

namespace Cyxor.Models
{
    public class IncludeApiModel
    {
        [Description("The navigation property name to include in the result.")]
        public string Property { get; set; }

        [Description("The criteria for the navigation property collection.")]
        public ReadApiModel Criteria { get; set; }
    }
}
