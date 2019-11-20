using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class RowColumnKeyApiModel
    {
        [Required]
        public RowKeyApiModel RowModel { get; set; }

        [Required]
        public ColumnKeyApiModel ColumnModel { get; set; }
    }
}
