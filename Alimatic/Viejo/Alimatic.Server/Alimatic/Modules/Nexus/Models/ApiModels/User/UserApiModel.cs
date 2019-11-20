namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    public class UserApiModel : NameAndIdApiModel
    {
        public int? AccountId { get; set; }
        public int SecurityId { get; set; }
    }
}
