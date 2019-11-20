using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class UpdateNameApiModel : NameOrIdApiModel
    {
        [Required]
        public string NewName { get; set; }
    }
}
