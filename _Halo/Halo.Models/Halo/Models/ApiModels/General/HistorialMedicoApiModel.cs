/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using Newtonsoft.Json;

namespace Halo.Models
{
    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class HistorialMedicoApiModel
    {
        public EgresoApiModel Egreso { get; set; }
        public AtencionPrenatalApiModel AtencionPrenatal { get; set; }
        public AtencionHospitalariaApiModel AtencionHospitalaria { get; set; }
        public AntecedenteGinecoObstetricoApiModel AntecedenteGinecoObstetrico { get; set; }
    }
}
/* { Halo.Server } */
