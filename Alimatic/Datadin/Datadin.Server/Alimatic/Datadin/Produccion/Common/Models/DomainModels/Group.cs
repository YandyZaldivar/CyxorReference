// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Datadin.Produccion.Models
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        public int DivisionId { get; set; }

        [ForeignKey(nameof(DivisionId))]
        public Division Division { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string Name { get; set; }

        [InverseProperty(nameof(Models.Enterprise.Group))]
        public HashSet<Enterprise> Enterprise { get; } = new HashSet<Enterprise>();
    }
}
// { Alimatic.Datadin } - Backend
