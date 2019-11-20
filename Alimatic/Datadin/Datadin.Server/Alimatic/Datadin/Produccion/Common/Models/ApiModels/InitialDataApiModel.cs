// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Collections.Generic;

namespace Alimatic.Datadin.Produccion.Models
{
    public class DataDinData
    {
        public IEnumerable<RowApiModel> Rows { get; set; }
        public IEnumerable<GroupApiModel> Groups { get; set; }
        public IEnumerable<ModelApiModel> Models { get; set; }
        public IEnumerable<DivisionApiModel> Divisions { get; set; }
        public IEnumerable<FrequencyApiModel> Frequencies { get; set; }
        public IEnumerable<EnterpriseApiModel> Enterprises { get; set; }
    }
}
// { Alimatic.Datadin } - Backend
