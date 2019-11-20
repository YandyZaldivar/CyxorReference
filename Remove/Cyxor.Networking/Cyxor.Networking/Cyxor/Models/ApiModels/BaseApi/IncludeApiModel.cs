using System.ComponentModel;

namespace Cyxor.Models
{
    public class IncludeApiModel
    {
        [Description("The property name to include.")]
        public string PropertyName { get; set; }

        public ListApiModel ListApiModel { get; set; }
    }
}
