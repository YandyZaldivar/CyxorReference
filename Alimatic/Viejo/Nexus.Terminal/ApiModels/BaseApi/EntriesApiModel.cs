using System.Collections.Generic;

namespace Alimatic.Nexus.Models
{
    public class EntriesApiModel<TApiModel>
    {
        public IEnumerable<TApiModel> Entries { get; set; }
    }
}
