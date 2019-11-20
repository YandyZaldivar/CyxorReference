using System.Collections.Generic;

namespace Alimatic.DataDin2.Models
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
