/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halo.Models
{
#if NET35
    using StringLengthAttribute = Cyxor.Models.StringLengthAttribute;
#endif

    public class Hospital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 4)]
        public string Nombre { get; set; }

        public int ProvinciaId { get; set; }

        [ForeignKey(nameof(ProvinciaId))]
        public Provincia Provincia { get; set; }

        [InverseProperty(nameof(Paciente.Hospital))]
        public List<Paciente> Pacientes { get; set; }
    }
}
/* { Halo.Server } */
