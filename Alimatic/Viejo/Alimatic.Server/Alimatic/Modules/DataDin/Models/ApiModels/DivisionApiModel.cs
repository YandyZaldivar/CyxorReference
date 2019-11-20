using System.Linq;
using System.Collections.Generic;

namespace Alimatic.DataDin.Models
{
    public class DivisionApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public IEnumerable<GrupoApiModel> Grupos { get; set; }

        public static DivisionApiModel[] Divisiones { get; } = new DivisionApiModel[]
        {
            new DivisionApiModel
            {
                Id = 1, Nombre = "Agroalimentaria",
                Grupos = new List<GrupoApiModel>(GrupoApiModel.Grupos.Where(p => p.DivisionId == 1))
            },

            new DivisionApiModel
            {
                Id = 2, Nombre = "Alimentaria",
                Grupos = new List<GrupoApiModel>(GrupoApiModel.Grupos.Where(p => p.DivisionId == 2))
            },

            new DivisionApiModel
            {
                Id = 3, Nombre = "Pesca",
                Grupos = new List<GrupoApiModel>(GrupoApiModel.Grupos.Where(p => p.DivisionId == 3))
            },

            new DivisionApiModel
            {
                Id = 4, Nombre = "Servicios",
                Grupos = new List<GrupoApiModel>(GrupoApiModel.Grupos.Where(p => p.DivisionId == 4))
            },
        };
    }
}
