using System.ComponentModel.DataAnnotations;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class MeasurementUnit : KeyApiModel<int>
    {
        [Required]
        [StringLength(126, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }
    }
}
