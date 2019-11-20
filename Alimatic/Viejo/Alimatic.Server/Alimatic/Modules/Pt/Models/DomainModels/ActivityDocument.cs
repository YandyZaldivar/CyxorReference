/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Pt.Models
{
    public class ActivityDocument
    {
        public int ActivityId { get; set; }

        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; }

        public int DocumentId { get; set; }

        [ForeignKey(nameof(DocumentId))]
        public Document Document { get; set; }
    }
}
/* { Alimatic.Server } */
