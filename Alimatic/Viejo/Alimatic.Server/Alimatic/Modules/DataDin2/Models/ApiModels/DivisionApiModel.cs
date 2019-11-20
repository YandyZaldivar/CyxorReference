using System.Linq;
using System.Collections.Generic;

namespace Alimatic.DataDin2.Models
{
    public class DivisionApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static DivisionApiModel[] Divisions { get; } = new DivisionApiModel[]
        {
            new DivisionApiModel { Id = 1, Name = "Agroalimentaria" },
            new DivisionApiModel { Id = 2, Name = "Alimentaria" },
            new DivisionApiModel { Id = 3, Name = "Pesca" },
            new DivisionApiModel { Id = 4, Name = "Servicios" },
        };

        //public IEnumerable<GroupApiModel> Groups { get; set; }

        //public static DivisionApiModel[] Divisions { get; } = new DivisionApiModel[]
        //{
        //    new DivisionApiModel
        //    {
        //        Id = 1, Name = "Agroalimentaria",
        //        Groups = new List<GroupApiModel>(GroupApiModel.Groups.Where(p => p.DivisionId == 1))
        //    },

        //    new DivisionApiModel
        //    {
        //        Id = 2, Name = "Alimentaria",
        //        Groups = new List<GroupApiModel>(GroupApiModel.Groups.Where(p => p.DivisionId == 2))
        //    },

        //    new DivisionApiModel
        //    {
        //        Id = 3, Name = "Pesca",
        //        Groups = new List<GroupApiModel>(GroupApiModel.Groups.Where(p => p.DivisionId == 3))
        //    },

        //    new DivisionApiModel
        //    {
        //        Id = 4, Name = "Servicios",
        //        Groups = new List<GroupApiModel>(GroupApiModel.Groups.Where(p => p.DivisionId == 4))
        //    },
        //};
    }
}
