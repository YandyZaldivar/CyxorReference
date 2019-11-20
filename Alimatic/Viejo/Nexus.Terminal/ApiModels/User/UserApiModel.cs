namespace Alimatic.Nexus.Models
{
    public class UserApiModel : NameAndIdApiModel
    {
        public int? AccountId { get; set; }
        public int SecurityId { get; set; }
    }
}
