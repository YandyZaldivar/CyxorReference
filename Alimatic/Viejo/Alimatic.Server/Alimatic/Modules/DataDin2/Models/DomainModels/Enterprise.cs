using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin2.Models
{
    public class Enterprise : IComparable<Enterprise>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int GroupId { get; set; }

        [ForeignKey("DivisionId, GroupId")]
        public Group Group { get; set; }

        public int DivisionId { get; set; }

        [ForeignKey(nameof(DivisionId))]
        public Division Division { get; set; }

        [StringLength(32, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(127, MinimumLength = 2)]
        public string FullName { get; set; }

        [InverseProperty(nameof(Record.Enterprise))]
        public HashSet<Record> EstadosFinancieros { get; } = new HashSet<Record>();

        public int CompareTo(Enterprise other)
        {
            if (DivisionId == other.DivisionId && GroupId == other.GroupId && Name == other.Name)
                return 0;

            if (DivisionId != other.DivisionId)
                return DivisionId.CompareTo(other.DivisionId);

            if (GroupId != other.GroupId)
                return GroupId.CompareTo(other.GroupId);

            return Name.CompareTo(other.Name);
        }
    }
}
