namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UserUpdate)]
    public class UserUpdateApiModel : UserKeyApiModel
    {
        public NameApiModel NewNameModel { get; set; }
        public AccountApiModel NewAccountModel { get; set; }
        public SecurityKeyApiModel NewSecurityModel { get; set; }
    }
}
