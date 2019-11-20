using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class EstadoFinancieroComparer : IComparer<EstadoFinanciero>
    {
        public int Compare(EstadoFinanciero x, EstadoFinanciero y)
        {
            if (x.Año == y.Año && x.Mes == y.Mes && x.EmpresaId == y.EmpresaId && x.ModeloId == y.ModeloId && x.FilaId == y.FilaId)
                return 0;

            var result = 0;

            if ((result = x.Año.CompareTo(y.Año)) != 0)
                return result;

            if ((result = x.Mes.CompareTo(y.Mes)) != 0)
                return result;

            if ((result = x.ModeloId.CompareTo(y.ModeloId)) != 0)
                return result;

            if (x.Empresa != null && y.Empresa != null)
                if ((result = x.Empresa.CompareTo(y.Empresa)) != 0)
                    return result;

            if ((result = x.EmpresaId.CompareTo(y.EmpresaId)) != 0)
                return result;

            return x.FilaId.CompareTo(y.FilaId);
        }
    }

    public class EstadoFinanciero
    {
        [Key]
        public int Año { get; set; }

        [Key]
        public int Mes { get; set; }

        [Key]
        public int EmpresaId { get; set; }

        [ForeignKey(nameof(EmpresaId))]
        public Empresa Empresa { get; set; }

        [Key]
        public int ModeloId { get; set; }

        [ForeignKey(nameof(ModeloId))]
        public Modelo Modelo { get; set; }

        [Key]
        public int FilaId { get; set; }

        [ForeignKey("FilaId, ModeloId")]
        public Fila Fila { get; set; }

        public decimal C1 { get; set; }

        public decimal C2 { get; set; }

        public decimal C3 { get; set; }
    }
}
