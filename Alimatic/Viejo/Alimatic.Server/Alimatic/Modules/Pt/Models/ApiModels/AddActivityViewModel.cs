/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;

namespace Alimatic.Pt.Models
{
    public class AddActivityViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ResponsibleId { get; set; }
        public Repetition Repetition { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
    }
}
/* { Alimatic.Server } */
