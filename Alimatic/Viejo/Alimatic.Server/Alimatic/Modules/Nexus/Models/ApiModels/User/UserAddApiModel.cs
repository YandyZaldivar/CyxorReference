namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    //[PacketConfig(ApiId.UserAdd)]
    public class UserAddApiModel : NameApiModel
    {
        public AccountApiModel AccountModel { get; set; }
        public SecurityKeyApiModel SecurityModel { get; set; }
    }
}
