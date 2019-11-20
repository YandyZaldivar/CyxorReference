using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Statistic : KeyApiModel<int>
    {
        public decimal LastPrice { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal MinimumPrice { get; set; }

        public decimal MaximumPrice { get; set; }

        public int InCount { get; set; }

        public int OutCount { get; set; }

        public decimal InAmount { get; set; }

        public decimal FifoOutAmount { get; set; }

        public decimal LifoOutAmount { get; set; }

        [NotMapped]
        public int Count => InCount - OutCount;

        [NotMapped]
        public decimal AverageOutAmount => InAmount - AverageAmount;

        [NotMapped]
        public decimal AverageAmount => AveragePrice * Count;

        [NotMapped]
        public decimal FifoAmount => InAmount - FifoOutAmount;

        [NotMapped]
        public decimal LifoAmount => InAmount - LifoOutAmount;

        /// <summary>
        /// The amount by which an entity's taxable income has been deferred by using the LIFO method.
        /// </summary>
        [NotMapped]
        public decimal LifoReserve => FifoAmount - LifoAmount;
    }
}
