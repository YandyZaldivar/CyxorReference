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

    public class Municipio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Nombre { get; set; }

        [Range(0, 20)]
        public int Codigo { get; set; }

        public int ProvinciaId { get; set; }

        [Required]
        [ForeignKey(nameof(ProvinciaId))]
        public Provincia Provincia { get; set; }

        [InverseProperty(nameof(Direccion.Municipio))]
        public virtual HashSet<Direccion> Direcciones { get; set; }

        public Municipio() => Direcciones = new HashSet<Direccion>();
    }
}
/* { Halo.Server } */
