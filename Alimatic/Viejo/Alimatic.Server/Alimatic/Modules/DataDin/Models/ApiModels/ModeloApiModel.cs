namespace Alimatic.DataDin.Models
{
    public class ModeloApiModel
    {
        public int Id { get; set; }
        public int CantidadFilas { get; set; }
        public int CantidadColumnas { get; set; }
        public string Descripcion { get; set; }

        public static ModeloApiModel[] Modelos { get; } = new ModeloApiModel[]
        {
            new ModeloApiModel { Id = 5920, CantidadFilas = 151, CantidadColumnas = 3, Descripcion = null },
            new ModeloApiModel { Id = 5921, CantidadFilas = 40, CantidadColumnas = 3, Descripcion = null },
            new ModeloApiModel { Id = 5924, CantidadFilas = 18, CantidadColumnas = 3, Descripcion = null },
            new ModeloApiModel { Id = 5925, CantidadFilas = 16, CantidadColumnas = 3, Descripcion = null },
            new ModeloApiModel { Id = 5926, CantidadFilas = 22, CantidadColumnas = 3, Descripcion = null },
        };
    }
}
