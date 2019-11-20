namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.AddUser)]
    public class AddUserApiModel : NameApiModel
    {
        public NameOrIdApiModel AccountModel { get; set; }
        public NameOrIdApiModel SecurityModel { get; set; }
    }
}
