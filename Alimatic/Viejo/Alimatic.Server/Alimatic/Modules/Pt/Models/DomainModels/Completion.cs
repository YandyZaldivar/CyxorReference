/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;

namespace Alimatic.Pt.Models
{
    public class Completion
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public List<WorkerActivityCompletion> WorkerActivities { get; set; }
    }
}
/* { Alimatic.Server } */
