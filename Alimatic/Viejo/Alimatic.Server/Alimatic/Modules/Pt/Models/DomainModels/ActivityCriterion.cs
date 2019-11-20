/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Pt.Models
{
    public class ActivityCriterion
    {
        public int ActivityId { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; }

        public int CriterionId { get; set; }

        [ForeignKey(nameof(CriterionId))]
        public Criterion Criterion { get; set; }

        public int SubCriterionId { get; set; }

        public SubCriterion SubCriterion { get; set; }
    }
}
/* { Alimatic.Server } */
