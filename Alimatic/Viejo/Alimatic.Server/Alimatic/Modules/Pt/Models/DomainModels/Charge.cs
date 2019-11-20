/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations;

namespace Alimatic.Pt.Models
{
    public class Charge
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(127, ErrorMessage = "The maximum allowable length of name column is 127")]
        public string Name { get; set; }

        public Worker Worker { get; set; }
    }
}
/* { Alimatic.Server } */
