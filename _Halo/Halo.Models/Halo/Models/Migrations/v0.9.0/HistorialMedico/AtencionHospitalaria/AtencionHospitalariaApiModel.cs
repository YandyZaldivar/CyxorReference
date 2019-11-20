/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Models.Migrations.V0900
{
    public class AtencionHospitalariaApiModel
    {
        public int? PartoId { get; set; }
        public int? HemorragiaId { get; set; }
        public int? MorbilidadPartoId { get; set; }
        public bool? UsoSulfatoMagnesio { get; set; }

        public int? LugarIngresoFlags { get; set; }
        public int? CausasMorbilidadFlags { get; set; }
        public int? IntervencionQuirurgicaFlags { get; set; }
        public int? UsoOcitocicosFlags { get; set; }

        public string OtraIntervencionQuirurgica { get; set; }

        public CriterioMorbilidadApiModel CriterioMorbilidad { get; set; }
    }
}
/* { Halo.Server } */
