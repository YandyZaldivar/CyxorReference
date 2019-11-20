using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin2.Models
{
    public class Division
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string Name { get; set; }

        [InverseProperty(nameof(Models.Group.Division))]
        public HashSet<Group> Group { get; } = new HashSet<Group>();

        [InverseProperty(nameof(Models.Enterprise.Division))]
        public HashSet<Enterprise> Enterprise { get; } = new HashSet<Enterprise>();
    }
}
