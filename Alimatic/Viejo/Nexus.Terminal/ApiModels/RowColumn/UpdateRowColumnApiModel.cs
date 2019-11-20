using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    using Cyxor.Models;

    [Model(ApiId.UpdateRowColumn)]
    public class UpdateRowColumnApiModel : GetRowColumnApiModel
    {
        [Required]
        public ApiModel<byte[]> NewValueModel { get; set; }
    }
}
