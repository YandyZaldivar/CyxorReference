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

    public class Provincia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Nombre { get; set; }

        [Required]
        [InverseProperty(nameof(Municipio.Provincia))]
        public virtual HashSet<Municipio> Municipios { get; set; }

        public Provincia() => Municipios = new HashSet<Municipio>();
    }
}
/* { Halo.Server } */
