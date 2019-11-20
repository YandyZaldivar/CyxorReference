//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

//using Newtonsoft.Json;

//namespace Cardyan.Inventory.Models
//{
//    public class WarehouseProductApiModel
//    {
//        [Key]
//        public int WarehouseId { get; set; }

//        [Key]
//        public int ProductId { get; set; }

//        public int? CategoryId { get; set; }

//        public int? LocationId { get; set; }

//        [StringLength(16380)]
//        public string Description { get; set; }

//        public int? Minimum { get; set; }

//        public int? Maximum { get; set; }

//        public WarehouseProductStatisticApiModel Statistic { get; set; } = new WarehouseProductStatisticApiModel();
//    }
//}
