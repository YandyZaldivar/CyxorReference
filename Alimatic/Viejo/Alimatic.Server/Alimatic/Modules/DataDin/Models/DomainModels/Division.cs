using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class Division
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string Nombre { get; set; }

        [InverseProperty(nameof(Grupo.Division))]
        public HashSet<Grupo> Grupos { get; } = new HashSet<Grupo>();

        [InverseProperty(nameof(Empresa.Division))]
        public HashSet<Empresa> Empresas { get; } = new HashSet<Empresa>();
    }
}
