//using System.ComponentModel.DataAnnotations;

//namespace Cardyan.Inventory.Models
//{
//    public class MovementProductApiModel
//    {
//        [Key]
//        public int MovementId { get; set; }

//        [Key]
//        public int ProductId { get; set; }

//        public bool TakeFromMinimum { get; set; }

//        public int Count { get; set; }

//        public decimal? Price { get; set; }

//        public decimal Amount => (Price ?? 0) * Count;

//        public decimal? FifoAmount { get; set; }
//        public decimal? LifoAmount { get; set; }
//        public decimal? AverageAmount { get; set; }
//    }
//}
