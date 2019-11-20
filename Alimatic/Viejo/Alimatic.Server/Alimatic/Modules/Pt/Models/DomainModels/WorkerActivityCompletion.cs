/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Pt.Models
{
    public class WorkerActivityCompletion
    {
        public int WorkerActivityWorkerId { get; set; }

        public int WorkerActivityActivityId { get; set; }

        public WorkerActivity WorkerActivity { get; set; }

        public int CompletionId { get; set; }

        [ForeignKey(nameof(CompletionId))]
        public Completion Completion { get; set; }

        public string Observations { get; set; }
    }
}
/* { Alimatic.Server } */
