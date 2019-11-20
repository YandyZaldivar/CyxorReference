//using System.ComponentModel.DataAnnotations;

//namespace Alimatic.Nexus.Models
//{
//    public class NameOrIdApiModel
//    {
//        [Required]
//        [MaxLength(32)]
//        public string NameOrId { get; set; }

//        public bool IsId => Id != null;
//        public string Name => IsId ? null : NameOrId;
//        public int? Id => int.TryParse(NameOrId, out var id) ? (int?)id : null;
//    }
//}
