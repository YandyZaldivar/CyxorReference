using System.ComponentModel.DataAnnotations;

namespace Alimatic.Nexus.Models
{
    public class NameOrIdApiModel
    {
        [Required]
        [MaxLength(32)]
        public string NameOrId { get; set; }

        public bool IsId => Id != null; //int.TryParse(NameOrId, out var id);

        public int? Id
        {
            get => int.TryParse(NameOrId, out var id) ? (int?)id : null;
            set => NameOrId = value.ToString();
        }

        public string Name
        {
            get => IsId ? null : NameOrId;
            set => NameOrId = value;
        }
    }
}
