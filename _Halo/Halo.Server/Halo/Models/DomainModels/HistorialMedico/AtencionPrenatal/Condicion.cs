/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
    public class Condicion
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionPrenatal AtencionPrenatal { get; set; }

        public bool Asma { get; set; }

        public bool Anemia { get; set; }

        public bool Prematuridad { get; set; }

        public bool EdadExtrema { get; set; }

        public bool Gemelaridad { get; set; }

        public bool PreEclampsia { get; set; }

        public bool Malnutricion { get; set; }

        public bool HabitosToxicos { get; set; }

        public bool DiabetesMellitus { get; set; }

        public bool InfeccionVaginal { get; set; }

        public bool InfeccionUrinaria { get; set; }

        public bool HipertensionArterial { get; set; }

        public bool InfeccionTransmisionSexual { get; set; }

        public bool Otros { get; set; }
    }
}
/* { Halo.Server } */
