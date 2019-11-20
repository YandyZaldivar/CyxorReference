/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Pt.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        public bool Enabled { get; set; } = true;

        public int RepetitionId { get; set; }

        [ForeignKey(nameof(RepetitionId))]
        public Repetition Repetition { get; set; }

        public DateTime ListedDate { get; set; }

        public DateTime? EndingDate { get; set; }

        [Required]
        public string Description { get; set; }

        public List<WorkerActivity> Workers { get; set; }

        public List<ActivityCriterion> Criteria { get; set; }

        public List<ActivityDocument> Documents { get; set; }
    }
}
/* { Alimatic.Server } */
