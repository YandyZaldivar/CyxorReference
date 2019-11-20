using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Coralsa.Models
{
    public class Frequency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
