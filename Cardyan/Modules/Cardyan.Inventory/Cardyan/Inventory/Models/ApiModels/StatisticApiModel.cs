//namespace Cardyan.Inventory.Models
//{
//    public class StatisticApiModel
//    {
//        public decimal LastPrice { get; set; }

//        public decimal AveragePrice { get; set; }

//        public decimal MinimumPrice { get; set; }

//        public decimal MaximumPrice { get; set; }

//        public int InCount { get; set; }

//        public int OutCount { get; set; }

//        public decimal InAmount { get; set; }

//        public decimal FifoOutAmount { get; set; }

//        public decimal LifoOutAmount { get; set; }

//        public int Count => InCount - OutCount;

//        public decimal AverageOutAmount => InAmount - AverageAmount;

//        public decimal AverageAmount => AveragePrice * Count;

//        public decimal FifoAmount => InAmount - FifoOutAmount;

//        public decimal LifoAmount => InAmount - LifoOutAmount;

//        public decimal LifoReserve => FifoAmount - LifoAmount;
//    }
//}
