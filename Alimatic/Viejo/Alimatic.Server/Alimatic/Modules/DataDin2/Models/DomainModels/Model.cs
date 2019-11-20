using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin2.Models
{
    public class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public int FrequencyId { get; set; }

        public bool IsEFModel { get; set; }

        [StringLength(1024)]
        public string ColumnNames { get; set; }

        [ForeignKey(nameof(FrequencyId))]
        public Frequency Frequency { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        [InverseProperty(nameof(Template.Model))]
        public virtual HashSet<Template> Templates { get; set; } = new HashSet<Template>();

        [InverseProperty(nameof(UserModel.Model))]
        public virtual HashSet<UserModel> Users { get; set; } = new HashSet<UserModel>();
    }
}