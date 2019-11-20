using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.GetRowColumn)]
    public class GetRowColumnApiModel
    {
        [Required]
        public IdApiModel RowModel { get; set; }

        [Required]
        public NameOrIdApiModel ColumnModel { get; set; }
    }
}
