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
    public class IntervencionQuirurgica
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Id))]
        public AtencionHospitalaria AtencionHospitalaria { get; set; }

        public bool SuturasCompresivas { get; set; }

        public bool HisterectomiaTotal { get; set; }

        public bool HisterectomiaSubTotal { get; set; }

        public bool SalpingectomiaTotalBilateral { get; set; }

        public bool SalpingectomiaTotalUnilateral { get; set; }

        public bool LigadurasArterialesSelectivas { get; set; }

        public bool LigadurasArteriasHipogastricas { get; set; }
    }
}
/* { Halo.Server } */
