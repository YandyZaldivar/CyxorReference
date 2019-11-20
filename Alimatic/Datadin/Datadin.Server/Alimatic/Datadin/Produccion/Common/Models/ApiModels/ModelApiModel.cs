// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

namespace Alimatic.Datadin.Produccion.Models
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
    }
}
// { Alimatic.Datadin } - Backend
