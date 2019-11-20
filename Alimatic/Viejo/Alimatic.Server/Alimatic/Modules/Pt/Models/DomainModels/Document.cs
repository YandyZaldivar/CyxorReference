/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.Collections.Generic;

namespace Alimatic.Pt.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public List<ActivityDocument> Activities { get; set; }
    }
}
/* { Alimatic.Server } */
