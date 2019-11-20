/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Models
{
    using Cyxor.Models;

    public class ProvinciaApiModel : IdNombreApiModel
    {
        public string NombreCorto { get; set; }

        public static ProvinciaApiModel[] Provincias { get; } = new ProvinciaApiModel[]
        {
            new ProvinciaApiModel { Id = 1, NombreCorto = "PRI", Nombre = "Pinar del Río" },
            new ProvinciaApiModel { Id = 2, NombreCorto = "ART", Nombre = "Artemisa" },
            new ProvinciaApiModel { Id = 3, NombreCorto = "HAB", Nombre = "La Habana" },
            new ProvinciaApiModel { Id = 4, NombreCorto = "MAY", Nombre = "Mayabeque" },
            new ProvinciaApiModel { Id = 5, NombreCorto = "MTZ", Nombre = "Matanzas" },
            new ProvinciaApiModel { Id = 6, NombreCorto = "CFG", Nombre = "Cienfuegos" },
            new ProvinciaApiModel { Id = 7, NombreCorto = "VCL", Nombre = "Villa Clara" },
            new ProvinciaApiModel { Id = 8, NombreCorto = "SSP", Nombre = "Sancti Spíritus" },
            new ProvinciaApiModel { Id = 9, NombreCorto = "CAV", Nombre = "Ciego de Ávila" },
            new ProvinciaApiModel { Id = 10, NombreCorto = "CMG", Nombre = "Camagüey" },
            new ProvinciaApiModel { Id = 11, NombreCorto = "LTU", Nombre = "Las Tunas" },
            new ProvinciaApiModel { Id = 12, NombreCorto = "HLG", Nombre = "Holguín" },
            new ProvinciaApiModel { Id = 13, NombreCorto = "GRM", Nombre = "Granma" },
            new ProvinciaApiModel { Id = 14, NombreCorto = "SCU", Nombre = "Santiago de Cuba" },
            new ProvinciaApiModel { Id = 15, NombreCorto = "GTM", Nombre = "Guantánamo" },
            new ProvinciaApiModel { Id = 16, NombreCorto = "IJV", Nombre = "Isla de la Juventud" },
        };
    }
}
/* { Halo.Server } */
