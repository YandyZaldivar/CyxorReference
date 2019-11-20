using System.Collections.Generic;

namespace Alimatic.Coralsa.Models
{
    public class InitialDataApiModel
    {
        public IEnumerable<RowApiModel> Rows { get; set; }
        public IEnumerable<GroupApiModel> Groups { get; set; }
        public IEnumerable<ModelApiModel> Models { get; set; }
        public IEnumerable<DivisionApiModel> Divisions { get; set; }
        public IEnumerable<FrequencyApiModel> Frequencies { get; set; }
        public IEnumerable<EnterpriseApiModel> Enterprises { get; set; }
    }
}
