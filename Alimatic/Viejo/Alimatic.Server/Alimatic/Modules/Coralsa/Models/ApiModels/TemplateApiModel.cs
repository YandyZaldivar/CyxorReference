namespace Alimatic.Coralsa.Models
{
    public class TemplateApiModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int ModelId { get; set; }
        public bool Locked { get; set; }

        public int EnterpriseId { get; set; }
        public string FileName { get; set; }
        public string FileData { get; set; }

        public static TemplateApiModel[] Templates { get; } = new TemplateApiModel[] { };
    }
}
