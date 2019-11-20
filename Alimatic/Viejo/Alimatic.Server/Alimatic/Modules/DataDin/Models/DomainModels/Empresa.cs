using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class Empresa : IComparable<Empresa>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int GrupoId { get; set; }

        [ForeignKey("DivisionId, GrupoId")]
        public Grupo Grupo { get; set; }

        public int DivisionId { get; set; }

        [ForeignKey(nameof(DivisionId))]
        public Division Division { get; set; }

        [StringLength(32, MinimumLength = 2)]
        public string Nombre { get; set; }

        [StringLength(127, MinimumLength = 2)]
        public string NombreCompleto { get; set; }

        [InverseProperty(nameof(EstadoFinanciero.Empresa))]
        public HashSet<EstadoFinanciero> EstadosFinancieros { get; } = new HashSet<EstadoFinanciero>();

        public int CompareTo(Empresa other)
        {
            if (DivisionId == other.DivisionId && GrupoId == other.GrupoId && Nombre == other.Nombre)
                return 0;

            if (DivisionId != other.DivisionId)
                return DivisionId.CompareTo(other.DivisionId);

            if (GrupoId != other.GrupoId)
                return GrupoId.CompareTo(other.GrupoId);

            return Nombre.CompareTo(other.Nombre);
        }
    }
}
