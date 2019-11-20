using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class NameApiModel
    {
        [Required]
        public string Name { get; set; }
    }
}
