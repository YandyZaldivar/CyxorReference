namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateUser)]
    public class UpdateUserApiModel : UserKeyApiModel
    {
        public NameApiModel NewNameModel { get; set; }
        public NameOrIdApiModel NewAccountModel { get; set; }
        public NameOrIdApiModel NewSecurityModel { get; set; }
    }
}
