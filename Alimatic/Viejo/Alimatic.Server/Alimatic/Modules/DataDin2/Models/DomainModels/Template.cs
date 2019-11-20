using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin2.Models
{
    public class Template
    {
        [Key]
        public int Year { get; set; }

        [Key]
        public int Month { get; set; }

        [Key]
        public int Day { get; set; }

        [Key]
        public int ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        [AutoMapper.IgnoreMap]
        public Model Model { get; set; }

        public bool Locked { get; set; }
    }
}
