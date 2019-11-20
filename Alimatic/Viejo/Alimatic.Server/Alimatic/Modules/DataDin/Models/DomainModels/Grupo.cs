using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class Grupo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        public int DivisionId { get; set; }

        [ForeignKey(nameof(DivisionId))]
        public Division Division { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string Nombre { get; set; }

        [InverseProperty(nameof(Empresa.Grupo))]
        public HashSet<Empresa> Empresas { get; } = new HashSet<Empresa>();
    }
}
