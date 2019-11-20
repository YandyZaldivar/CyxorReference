using System.ComponentModel;

namespace Cyxor.Models
{
    public class OrderByApiModel
    {
        [Description("The property name used to order the elements.")]
        public string PropertyName { get; set; }

        [Description("If 'true', applies the order in descending order. The default is ascending.")]
        public bool Descending { get; set; }
    }
}
