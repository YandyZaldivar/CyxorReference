// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

namespace Alimatic.Datadin.Produccion.Models
{
    public class DivisionApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static DivisionApiModel[] GeiaDivisions { get; } = new DivisionApiModel[]
        {
            new DivisionApiModel { Id = 1, Name = "Agroalimentaria" },
            new DivisionApiModel { Id = 2, Name = "Alimentaria" },
            new DivisionApiModel { Id = 3, Name = "Pesca" },
            new DivisionApiModel { Id = 4, Name = "Servicios" },
        };

        public static DivisionApiModel[] MinalDivisions { get; } = new DivisionApiModel[]
        {
            new DivisionApiModel { Id = 1, Name = "Minal" },
        };
    }
}
// { Alimatic.Datadin } - Backend
