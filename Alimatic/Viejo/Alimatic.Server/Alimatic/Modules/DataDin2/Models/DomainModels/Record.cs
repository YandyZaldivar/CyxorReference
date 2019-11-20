using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin2.Models
{
    public class RegistrosComparer : IComparer<Record>
    {
        public int Compare(Record x, Record y)
        {
            if (x.Year == y.Year && x.Month == y.Month && x.Day == y.Day && x.EnterpriseId == y.EnterpriseId && x.ModelId == y.ModelId && x.RowId == y.RowId)
                return 0;

            var result = 0;

            if ((result = x.Year.CompareTo(y.Year)) != 0)
                return result;

            if ((result = x.Month.CompareTo(y.Month)) != 0)
                return result;

            if ((result = x.Day.CompareTo(y.Day)) != 0)
                return result;

            if ((result = x.ModelId.CompareTo(y.ModelId)) != 0)
                return result;

            if (x.Enterprise != null && y.Enterprise != null)
                if ((result = x.Enterprise.CompareTo(y.Enterprise)) != 0)
                    return result;

            if ((result = x.EnterpriseId.CompareTo(y.EnterpriseId)) != 0)
                return result;

            return x.RowId.CompareTo(y.RowId);
        }
    }

    public class Record
    {
        [Key]
        public int Year { get; set; }

        [Key]
        public int Month { get; set; }

        [Key]
        public int Day { get; set; }

        [Key]
        public int EnterpriseId { get; set; }

        [ForeignKey(nameof(EnterpriseId))]
        public Enterprise Enterprise { get; set; }

        [Key]
        public int ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public Model Model { get; set; }

        [Key]
        public int RowId { get; set; }

        [ForeignKey("RowId, ModelId")]
        public Row Row { get; set; }

        [ForeignKey("Year, Month, Day, ModelId")]
        public Template Template { get; set; }

        public decimal? C01 { get; set; }

        public decimal? C02 { get; set; }

        public decimal? C03 { get; set; }

        public decimal? C04 { get; set; }

        public decimal? C05 { get; set; }

        public decimal? C06 { get; set; }

        public decimal? C07 { get; set; }

        public decimal? C08 { get; set; }

        public decimal? C09 { get; set; }

        public override string ToString() =>
            $"{{\n{nameof(Year)} = {Year}\n{nameof(Month)} = {Month}\n" +
            $"{nameof(Day)} = {Day}\n{nameof(Enterprise)} = {EnterpriseId}\n" +
            $"{nameof(Model)} = {ModelId}\n{nameof(Row)} = {RowId}";
    }
}
