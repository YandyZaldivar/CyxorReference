namespace Alimatic.DataDin2.Models
{
    public class ModelApiModel
    {
        public int Id { get; set; }
        public int RowCount { get; set; }
        public bool IsEFModel { get; set; }
        public int ColumnCount { get; set; }
        public int FrequencyId { get; set; }
        public string ColumnNames { get; set; }
        public string Description { get; set; }

        public static ModelApiModel[] Models { get; } = new ModelApiModel[]
        {
            //new ModelApiModel { Id = 5920, RowCount = 151, ColumnCount = 3, Description = null },
            //new ModelApiModel { Id = 5921, RowCount = 40, ColumnCount = 3, Description = null },
            //new ModelApiModel { Id = 5924, RowCount = 18, ColumnCount = 3, Description = null },
            //new ModelApiModel { Id = 5925, RowCount = 16, ColumnCount = 3, Description = null },
            //new ModelApiModel { Id = 5926, RowCount = 22, ColumnCount = 3, Description = null },
        };
    }
}
