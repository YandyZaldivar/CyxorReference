namespace Alimatic.DataDin2.Models
{
    public class UserApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? AccountId { get; set; }
        public string Password { get; set; }
        public int Permission { get; set; }
        public int? EnterpriseId { get; set; }
        public int SecurityLevel { get; set; }
    }
}
