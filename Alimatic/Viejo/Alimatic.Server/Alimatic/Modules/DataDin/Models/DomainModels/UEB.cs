using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class UEB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int EmpresaId { get; set; }

        [ForeignKey(nameof(EmpresaId))]
        public Empresa Empresa { get; set; }

        [StringLength(32, MinimumLength = 2)]
        public string Nombre { get; set; }

        [StringLength(128, MinimumLength = 2)]
        public string NombreCompleto { get; set; }

        [InverseProperty(nameof(EstadoFinanciero.Empresa))]
        public HashSet<EstadoFinanciero> EstadosFinancieros { get; } = new HashSet<EstadoFinanciero>();
    }
}
