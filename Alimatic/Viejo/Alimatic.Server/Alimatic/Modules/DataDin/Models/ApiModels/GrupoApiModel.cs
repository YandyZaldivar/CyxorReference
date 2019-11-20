namespace Alimatic.DataDin.Models
{
    public class GrupoApiModel
    {
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public string Nombre { get; set; }

        public static GrupoApiModel[] Grupos { get; } = new GrupoApiModel[]
        {
            new GrupoApiModel { Id = 1, DivisionId = 1, Nombre = "Carnes" },
            new GrupoApiModel { Id = 2, DivisionId = 1, Nombre = "Lácteos" },
            new GrupoApiModel { Id = 3, DivisionId = 1, Nombre = "Otras" },

            new GrupoApiModel { Id = 1, DivisionId = 2, Nombre = "Bebidas" },
            new GrupoApiModel { Id = 2, DivisionId = 2, Nombre = "Cervezas" },
            new GrupoApiModel { Id = 3, DivisionId = 2, Nombre = "Aceites" },
            new GrupoApiModel { Id = 4, DivisionId = 2, Nombre = "Otras" },

            new GrupoApiModel { Id = 1, DivisionId = 3, Nombre = "Plataforma" },
            new GrupoApiModel { Id = 2, DivisionId = 3, Nombre = "Acuicultura" },
            new GrupoApiModel { Id = 3, DivisionId = 3, Nombre = "Otras" },

            new GrupoApiModel { Id = 1, DivisionId = 4, Nombre = "Servicios" },
        };
    }
}
