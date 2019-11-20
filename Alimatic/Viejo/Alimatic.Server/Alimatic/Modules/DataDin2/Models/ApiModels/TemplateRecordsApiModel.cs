using System.Collections.Generic;

namespace Alimatic.DataDin2.Models
{
    public class TemplateRecordsApiModel
    {
        public TemplateApiModel Template { get; set; }
        public IEnumerable<RecordColumnsApiModel> Records { get; set; }
    }
}
