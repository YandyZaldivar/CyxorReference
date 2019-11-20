// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

namespace Alimatic.Datadin.Produccion.Models
{
    public class FrequencyApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static FrequencyApiModel[] Frequencies { get; } = new FrequencyApiModel[]
        {
            new FrequencyApiModel { Id = 1, Name = "Daily" },
            new FrequencyApiModel { Id = 2, Name = "Monthly" },
        };
    }
}
// { Alimatic.Datadin } - Backend
