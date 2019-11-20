// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.Collections.Generic;

namespace Alimatic.Datadin.Produccion.Models
{
    public class TemplateRecordsApiModel
    {
        public TemplateApiModel Template { get; set; }
        public IEnumerable<RecordColumnsApiModel> Records { get; set; }
    }
}
// { Alimatic.Datadin } - Backend
