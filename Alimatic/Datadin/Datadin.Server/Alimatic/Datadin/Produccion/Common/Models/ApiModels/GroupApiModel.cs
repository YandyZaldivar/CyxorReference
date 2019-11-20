// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

namespace Alimatic.Datadin.Produccion.Models
{
    public class GroupApiModel
    {
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public string Name { get; set; }

        public static GroupApiModel[] GeiaGroups { get; } = new GroupApiModel[]
        {
            new GroupApiModel { Id = 1, DivisionId = 1, Name = "Carnes" },
            new GroupApiModel { Id = 2, DivisionId = 1, Name = "Lácteos" },
            new GroupApiModel { Id = 3, DivisionId = 1, Name = "Otras" },

            new GroupApiModel { Id = 1, DivisionId = 2, Name = "Bebidas" },
            new GroupApiModel { Id = 2, DivisionId = 2, Name = "Cervezas" },
            new GroupApiModel { Id = 3, DivisionId = 2, Name = "Aceites" },
            new GroupApiModel { Id = 4, DivisionId = 2, Name = "Otras" },

            new GroupApiModel { Id = 1, DivisionId = 3, Name = "Plataforma" },
            new GroupApiModel { Id = 2, DivisionId = 3, Name = "Acuicultura" },
            new GroupApiModel { Id = 3, DivisionId = 3, Name = "Otras" },

            new GroupApiModel { Id = 1, DivisionId = 4, Name = "Servicios" },
        };

        public static GroupApiModel[] MinalGroups { get; } = new GroupApiModel[]
        {
            new GroupApiModel { Id = 1, DivisionId = 1, Name = "Coralsa" },
            new GroupApiModel { Id = 2, DivisionId = 1, Name = "Cubaron" },
            new GroupApiModel { Id = 3, DivisionId = 1, Name = "Empleadoras" },
            new GroupApiModel { Id = 4, DivisionId = 1, Name = "Presupuestadas" },
        };
    }
}
// { Alimatic.Datadin } - Backend
