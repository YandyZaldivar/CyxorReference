using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.AddRow)]
    public class AddRowApiModel
    {
        public NameOrIdApiModel UserModel { get; set; }

        [Required]
        public NameOrIdApiModel TableModel { get; set; }
    }
}
