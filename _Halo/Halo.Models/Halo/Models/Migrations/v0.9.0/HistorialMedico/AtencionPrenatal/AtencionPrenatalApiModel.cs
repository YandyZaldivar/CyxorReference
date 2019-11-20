/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Models.Migrations.V0900
{
    public class AtencionPrenatalApiModel
    {
        public bool? Reevaluacion { get; set; }
        public int? SemanasCaptacion { get; set; }
        public int? ControlesPrenatales { get; set; }
        public bool? EvaluadoComoRiesgo { get; set; }
        public int? IndiceMasaCorporalId { get; set; }
        public int? DopplerArteriaUterina { get; set; }
        public int? CondicionesIdentificadasFlags { get; set; }

        public UrocultivoApiModel Urocultivo { get; set; }
        public HemoglobinaApiModel Hemoglobina { get; set; }
    }
}
/* { Halo.Server } */
