/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Pt.Models
{
    public class SubCriterion
    {
        public int CriterionId { get; set; }

        [ForeignKey(nameof(CriterionId))]
        public Criterion Criterion { get; set; }

        public int SubCriterionId { get; set; }

        [MaxLength(127, ErrorMessage = "The maximum allowable length of name column is 127")]
        public string Name { get; set; }

        [InverseProperty(nameof(ActivityCriterion.SubCriterion))]
        public List<ActivityCriterion> Activities { get; set; }
    }
}
/* { Alimatic.Server } */
