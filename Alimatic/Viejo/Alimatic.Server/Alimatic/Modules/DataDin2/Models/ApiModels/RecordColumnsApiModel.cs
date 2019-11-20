namespace Alimatic.DataDin2.Models
{
    public class RecordColumnsApiModel
    {
        public int RowId { get; set; }

        public decimal? C01 { get; set; }
        public decimal? C02 { get; set; }
        public decimal? C03 { get; set; }
        public decimal? C04 { get; set; }
        public decimal? C05 { get; set; }
        public decimal? C06 { get; set; }
        public decimal? C07 { get; set; }
        public decimal? C08 { get; set; }
        public decimal? C09 { get; set; }

        public bool C01Edit { get; set; } = true;
        public bool C02Edit { get; set; } = true;
        public bool C03Edit { get; set; } = true;
        public bool C04Edit { get; set; } = true;
        public bool C05Edit { get; set; } = true;
        public bool C06Edit { get; set; } = true;
        public bool C07Edit { get; set; } = true;
        public bool C08Edit { get; set; } = true;
        public bool C09Edit { get; set; } = true;

        public void SetEditMode(bool edit)
        {
            C01Edit = edit;
            C02Edit = edit;
            C03Edit = edit;
            C04Edit = edit;
            C05Edit = edit;
            C06Edit = edit;
            C07Edit = edit;
            C08Edit = edit;
            C09Edit = edit;
        }
    }
}
