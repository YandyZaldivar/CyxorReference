using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class FilaComparer : IComparer<Fila>
    {
        public int Compare(Fila x, Fila y)
        {
            if (x.Id == y.Id && x.ModeloId == y.ModeloId)
                return 0;

            return x.ModeloId == y.ModeloId ? x.Id.CompareTo(y.ModeloId) : x.ModeloId.CompareTo(y.ModeloId);
        }
    }

    public class Fila
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        public int ModeloId { get; set; }

        [ForeignKey(nameof(ModeloId))]
        public Modelo Modelo { get; set; }

        [MaxLength(1024)]
        public string Descripcion { get; set; }

        public override string ToString() => $"{{ {nameof(Id)}: {Id}, {nameof(ModeloId)}: {ModeloId} }}";
    }
}
