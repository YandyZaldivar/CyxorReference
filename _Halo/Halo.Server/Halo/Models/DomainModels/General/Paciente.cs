/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Halo.Models
{
    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        public int? Edad { get; set; }

        [StringLength(64)]
        public string Nombre { get; set; }

        //public int? HistoriaClinica { get; set; }
        public int? HistoriaClinica => Id == 0 ? null : (int?)(Id + 1_000_000);

        [DataType(DataType.Date)]
        public DateTime? FechaIngreso { get; set; }

        public int? OcupacionId { get; set; }

        [ForeignKey(nameof(OcupacionId))]
        public Ocupacion Ocupacion { get; set; }

        public int? EscolaridadId { get; set; }

        [ForeignKey(nameof(EscolaridadId))]
        public Escolaridad Escolaridad { get; set; }

        public int? HospitalId { get; set; }

        [ForeignKey(nameof(HospitalId))]
        public Hospital Hospital { get; set; }

        public int? Traslado1HospitalId { get; set; }

        [ForeignKey(nameof(Traslado1HospitalId))]
        public Hospital Traslado1Hospital { get; set; }

        public int? Traslado2HospitalId { get; set; }

        [ForeignKey(nameof(Traslado2HospitalId))]
        public Hospital Traslado2Hospital { get; set; }

        [InverseProperty(nameof(Models.Direccion.Paciente))]
        public Direccion Direccion { get; set; }

        [InverseProperty(nameof(Models.HistorialMedico.Paciente))]
        public HistorialMedico HistorialMedico { get; set; }

        public int? EstadiaHospitalaria
        {
            get
            {
                if (FechaIngreso == null)
                    return null;

                var fechaEgreso = HistorialMedico?.Egreso?.Fecha;
                return (int)(FechaIngreso.Value - (fechaEgreso ?? DateTime.Now.Date)).TotalDays;
            }
        }
    }
}
/* { Halo.Server } */
