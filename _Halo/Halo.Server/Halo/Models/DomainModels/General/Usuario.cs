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
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public bool Visualizador { get; set; }

        public int? HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public int? ProvinciaId { get; set; }

        public Provincia Provincia { get; set; }

        public bool Nacional => Hospital == null && Provincia == null;

        public bool Provincial => Hospital == null && Provincia != null;

        public bool Hospitalario => Hospital != null;
    }
}
/* { Halo.Server } */
