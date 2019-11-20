/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;

using Newtonsoft.Json;

namespace Halo.Models.Migrations.V0900
{
    public class AccountPacienteApiModel
    {
        public string Error { get; set; }

        public bool Visto { get; set; }
        public bool Seguimiento { get; set; }
    }

    [JsonObject(ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class PacienteApiModel
    {
        public AccountPacienteApiModel Account { get; set; } = new AccountPacienteApiModel();

        public bool SetAsDeleted { get; set; }

        public string Id { get; set; }
        public int? Edad { get; set; }
        public string Nombre { get; set; }
        public int? AreaId { get; set; }
        public int? HospitalId { get; set; }
        public int? OcupacionId { get; set; }
        public int? MunicipioId { get; set; }
        public int? EscolaridadId { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int? Traslado1HospitalId { get; set; }
        public int? Traslado2HospitalId { get; set; }

        public HistorialMedicoApiModel HistorialMedico { get; set; }

        public string HistoriaClinica => string.IsNullOrEmpty(Id) ? null : Id.GetHashCode().ToString("X");

        public int? EstadiaHospitalaria
        {
            get
            {
                if (FechaIngreso == null)
                    return null;

                var fechaEgreso = HistorialMedico?.Egreso?.Fecha;
                return (int)((fechaEgreso ?? DateTime.Now.Date) - FechaIngreso.Value).TotalDays;
            }
        }

        public string Validate()
        {
            var nombre = Nombre?.Trim();

            if (string.IsNullOrEmpty(nombre))
                return "El nombre de la paciente es obligatorio";

            if ((Edad ?? 0) == 0)
                return "La edad de la paciente es obligatoria";

            if (FechaIngreso == null)
                return "La fecha de ingreso de la paciente es obligatoria";

            if ((HistorialMedico?.Egreso?.Fecha ?? DateTime.Now) < FechaIngreso)
                return "La fecha de egreso no puede ser anterior a la fecha de ingreso";

            return null;
        }
    }
}
/* { Halo.Server } */
