using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin.Models
{
    public class Modelo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CantidadFilas { get; set; }

        public int CantidadColumnas { get; set; }

        [StringLength(1024)]
        public string Descripcion { get; set; }
    }
}