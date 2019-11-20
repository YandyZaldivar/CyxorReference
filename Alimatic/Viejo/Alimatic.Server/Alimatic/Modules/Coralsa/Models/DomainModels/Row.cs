using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Coralsa.Models
{
    public class RowComparer : IComparer<Row>
    {
        public int Compare(Row x, Row y)
        {
            if (x.Id == y.Id && x.ModelId == y.ModelId)
                return 0;

            return x.ModelId == y.ModelId ? x.Id.CompareTo(y.ModelId) : x.ModelId.CompareTo(y.ModelId);
        }
    }

    public class Row
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        public int ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public Model Model { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public override string ToString() => $"{{ {nameof(Id)}: {Id}, {nameof(ModelId)}: {ModelId} }}";
    }
}
