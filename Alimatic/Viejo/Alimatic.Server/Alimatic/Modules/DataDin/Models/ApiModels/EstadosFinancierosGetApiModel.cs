using System;

namespace Alimatic.DataDin.Models
{
    using Cyxor.Models;    

    /// <summary>
    /// Api: <c>datadin ef</c>. Obtiene los estados financieros según los criterios seleccionados de
    /// Año, Mes, División, Grupo, Empresa y Modelo.
    /// </summary>
    //[Model("datadin ef", Description = "Obtiene los estados financieros según los criterios seleccionados de Año, Mes, Empresa y Modelo")]
    public class EstadosFinancierosGetApiModel : IEquatable<EstadosFinancierosGetApiModel>
    {
        public int Year { get; set; } = DateTime.Now.Year;
        public int? Month { get; set; }
        public int? Division { get; set; }
        public int? Grupo { get; set; }
        public int? Row { get; set; }
        public int? Column { get; set; }
        public int? Empresa { get; set; }
        public int Model { get; set; } = 5920;

        public bool Equals(EstadosFinancierosGetApiModel other)
        {
            if (Year == other.Year &&
                Month == other.Month &&
                Division == other.Division &&
                Grupo == other.Grupo &&
                Row == other.Row &&
                Empresa == other.Empresa &&
                Model == other.Model)
                return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is EstadosFinancierosGetApiModel other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode() => Year + Month ?? 0 + Division ?? 0 + Grupo ?? 0 + Row ?? 0 + Empresa ?? 0 + Model;
    }
}
