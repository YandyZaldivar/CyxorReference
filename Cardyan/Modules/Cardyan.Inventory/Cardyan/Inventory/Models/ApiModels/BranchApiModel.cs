//using System.ComponentModel.DataAnnotations;

//using System.Collections.Generic;

//namespace Cardyan.Inventory.Models
//{
//    public class BranchApiModel
//    {
//        [Key]
//        public int Id { get; set; }

//        [StringLength(126, MinimumLength = 1)]
//        public string Code { get; set; }

//        [StringLength(126, MinimumLength = 2)]
//        public string Name { get; set; }

//        [StringLength(16380)]
//        public string Description { get; set; }

//        public HashSet<Warehouse> Warehouses { get; } = new HashSet<Warehouse>();
//    }
//}
