namespace Alimatic.DataDin.Models
{
    public class EmpresaApiModel
    {
        public int Id { get; set; }
        [AutoMapper.IgnoreMap]
        public int GrupoId { get; set; }
        public int DivisionId { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }

        public static EmpresaApiModel[] Empresas { get; } = new EmpresaApiModel[]
        {
            // Alimentaria

            /* 01 */ new EmpresaApiModel { Id = 1580, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Pinar del Río", NombreCompleto = "Empresa cárnica Pinar del Río" },
            /* 02 */ new EmpresaApiModel { Id = 1585, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Matanzas", NombreCompleto = "Empresa cárnica Matanzas" },
            /* 03 */ new EmpresaApiModel { Id = 1587, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Villa Clara", NombreCompleto = "Empresa cárnica Villa Clara" },
            /* 04 */ new EmpresaApiModel { Id = 1588, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Sancti Spíritus", NombreCompleto = "Empresa cárnica Sancti Spíritus" },
            /* 05 */ new EmpresaApiModel { Id = 1589, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Camagüey", NombreCompleto = "Empresa cárnica Camagüey" },
            /* 06 */ new EmpresaApiModel { Id = 1591, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Holguín", NombreCompleto = "Empresa cárnica Holguín" },
            /* 07 */ new EmpresaApiModel { Id = 1592, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Granma", NombreCompleto = "Empresa cárnica Granma" },
            /* 08 */ new EmpresaApiModel { Id = 1593, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Santiago de Cuba", NombreCompleto = "Empresa cárnica Santiago" },
            /* 09 */ new EmpresaApiModel { Id = 1954, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Guantánamo", NombreCompleto = "Empresa cárnica Guantánamo" },
            /* 10 */ new EmpresaApiModel { Id = 1961, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Ciego de Ávila", NombreCompleto = "Empresa cárnica Ciego de Ávila" },
            /* 11 */ new EmpresaApiModel { Id = 2660, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Las Tunas", NombreCompleto = "Empresa cárnica Las Tunas" },
            /* 12 */ new EmpresaApiModel { Id = 2664, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica La Habana", NombreCompleto = "Empresa cárnica Habana" },
            /* 13 */ new EmpresaApiModel { Id = 2756, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Cienfuegos", NombreCompleto = "Empresa cárnica Cienfuegos" },
            /* 14 */ new EmpresaApiModel { Id = 12700, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Tauro", NombreCompleto = "Empresa cárnica Tauro" },
            /* 15 */ new EmpresaApiModel { Id = 13017, DivisionId = 1, GrupoId = 1, Nombre = "Cárnica Nueva Paz", NombreCompleto = "Empresa cárnica Nueva Paz" },
            /* 16 */ new EmpresaApiModel { Id = 14203, DivisionId = 1, GrupoId = 1, Nombre = "ASECAR", NombreCompleto = "Empresa de aseguramiento de la industria cárnica" },

            /* 17 */ new EmpresaApiModel { Id = 1384, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Coppelia", NombreCompleto = "Empresa de productos lácteos Coppelia" },
            /* 18 */ new EmpresaApiModel { Id = 1594, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Pinar del Río", NombreCompleto = "Empresa de productos lácteos y confitería Pinar del Río" },
            /* 19 */ new EmpresaApiModel { Id = 1595, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Artemisa", NombreCompleto = "Empresa de productos lácteos Artemisa" },
            /* 20 */ new EmpresaApiModel { Id = 1597, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Aseguramiento", NombreCompleto = "Empresa de aseguramiento de la industria láctea" },
            /* 21 */ new EmpresaApiModel { Id = 1598, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos La Habana", NombreCompleto = "Empresa complejo lácteo Habana" },
            /* 22 */ new EmpresaApiModel { Id = 1599, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Matanzas", NombreCompleto = "Empresa de productos lácteos Matanzas" },
            /* 23 */ new EmpresaApiModel { Id = 1600, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Villa Clara", NombreCompleto = "Empresa de productos lácteos Villa Clara" },
            /* 24 */ new EmpresaApiModel { Id = 1601, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Escambray", NombreCompleto = "Empresa de productos lácteos Escambray" },
            /* 25 */ new EmpresaApiModel { Id = 1606, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Río Zaza", NombreCompleto = "Empresa de productos lácteos Río Zaza" },
            /* 26 */ new EmpresaApiModel { Id = 1607, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Ciego de Ávila", NombreCompleto = "Empresa de productos lácteos Ciego de Ávila" },
            /* 27 */ new EmpresaApiModel { Id = 1609, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Camagüey", NombreCompleto = "Empresa de productos lácteos Camagüey" },
            /* 28 */ new EmpresaApiModel { Id = 1610, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Las Tunas", NombreCompleto = "Empresa de productos lácteos Las Tunas" },
            /* 29 */ new EmpresaApiModel { Id = 1611, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Holguín", NombreCompleto = "Empresa de productos lácteos Holguín" },
            /* 30 */ new EmpresaApiModel { Id = 1612, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Bayamo", NombreCompleto = "Empresa de productos lácteos Bayamo" },
            /* 31 */ new EmpresaApiModel { Id = 1615, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Santiago de Cuba", NombreCompleto = "Empresa de productos lácteos Santiago de Cuba" },
            /* 32 */ new EmpresaApiModel { Id = 1616, DivisionId = 1, GrupoId = 2, Nombre = "Lácteos Guantánamo", NombreCompleto = "Empresa de productos lácteos Guantánamo" },

            /* 33 */ new EmpresaApiModel { Id = 14215, DivisionId = 1, GrupoId = 3, Nombre = "Conservas de Vegetales", NombreCompleto = "Empresa de conservas de vegetales" },
            /* 34 */ new EmpresaApiModel { Id = 14216, DivisionId = 1, GrupoId = 3, Nombre = "Torrefactora de Café", NombreCompleto = "Empresa de torrefacción y comercialización de café" },
            /* 35 */ new EmpresaApiModel { Id = 4272, DivisionId = 1, GrupoId = 3, Nombre = "Alimentos Isla", NombreCompleto = "Empresa productora de alimentos Isla de la Juventud" },

            /* 36 */ new EmpresaApiModel { Id = 1663, DivisionId = 2, GrupoId = 1, Nombre = "EMBER La Habana", NombreCompleto = "Empresa de bebidas y refrescos La Habana" },
            /* 37 */ new EmpresaApiModel { Id = 1665, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Villa Clara", NombreCompleto = "Empresa de bebidas y refrescos Villa Clara" },
            /* 38 */ new EmpresaApiModel { Id = 1666, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Camagüey", NombreCompleto = "Empresa de bebidas y refrescos Camagüey" },
            /* 39 */ new EmpresaApiModel { Id = 1667, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Granma", NombreCompleto = "Empresa de bebidas y refrescos Granma" },
            /* 40 */ new EmpresaApiModel { Id = 1668, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Santiago de Cuba", NombreCompleto = "Empresa de bebidas y refrescos Santiago de Cuba" },
            /* 41 */ new EmpresaApiModel { Id = 1674, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Pinar del Río", NombreCompleto = "Empresa de bebidas y refrescos Pinar del Río" },
            /* 42 */ new EmpresaApiModel { Id = 1679, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Ciego de Ávila", NombreCompleto = "Empresa de bebidas y refrescos Ciego de Ávila" },
            /* 43 */ new EmpresaApiModel { Id = 4796, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Mayabeque", NombreCompleto = "Empresa de bebidas y refrescos Mayabeque" },
            /* 44 */ new EmpresaApiModel { Id = 13042, DivisionId = 2, GrupoId = 1, Nombre = "EMBER Aseguramiento", NombreCompleto = "Empresa aseguramiento industrias bebidas y refrescos" },

            /* 45 */ new EmpresaApiModel { Id = 1672, DivisionId = 2, GrupoId = 2, Nombre = "Cervecería Manacas", NombreCompleto = "Empresa cervecería Antonio Díaz Santana (Manacas)" },
            /* 46 */ new EmpresaApiModel { Id = 3008, DivisionId = 2, GrupoId = 2, Nombre = "Cervecería Modelo", NombreCompleto = "Empresa cervecería Guido Pérez (Modelo)" },
            /* 47 */ new EmpresaApiModel { Id = 4175, DivisionId = 2, GrupoId = 2, Nombre = "Cervecería Hatuey", NombreCompleto = "Empresa cervecería Santiago de Cuba (Hatuey)" },
            /* 48 */ new EmpresaApiModel { Id = 11724, DivisionId = 2, GrupoId = 2, Nombre = "Cervecería Tinima", NombreCompleto = "Empresa cervecería Tinima" },

            /* 49 */ new EmpresaApiModel { Id = 1617, DivisionId = 2, GrupoId = 3, Nombre = "Aceites La Habana", NombreCompleto = "Empresa de aceites y grasas comestibles La Habana" },
            /* 50 */ new EmpresaApiModel { Id = 13480, DivisionId = 2, GrupoId = 3, Nombre = "Aceites Camagüey", NombreCompleto = "Empresa de aceites y grasas comestibles Camagüey" },
            /* 51 */ new EmpresaApiModel { Id = 2794, DivisionId = 2, GrupoId = 3, Nombre = "Erasol", NombreCompleto = "Empresa refinadora de aceites de Santiago de Cuba (Erasol)" },
            /* 52 */ new EmpresaApiModel { Id = 13819, DivisionId = 2, GrupoId = 3, Nombre = "PDS", NombreCompleto = "Empresa procesadora de soya (PDS)" },

            /* 53 */ new EmpresaApiModel { Id = 1734, DivisionId = 2, GrupoId = 4, Nombre = "Molinera", NombreCompleto = "Empresa cubana de molinería" },
            /* 54 */ new EmpresaApiModel { Id = 14194, DivisionId = 2, GrupoId = 4, Nombre = "Confitera", NombreCompleto = "Empresa de confitería y derivados de la harina" },
            /* 55 */ new EmpresaApiModel { Id = 11989, DivisionId = 2, GrupoId = 4, Nombre = "Cubana del Pan", NombreCompleto = "Empresa cubana del pan" },

            /* 56 */ new EmpresaApiModel { Id = 7508, DivisionId = 3, GrupoId = 1, Nombre = "PESCAHABANA", NombreCompleto = "Pesquera Industrial de Batabanó PESCAHABANA" },
            /* 57 */ new EmpresaApiModel { Id = 7892, DivisionId = 3, GrupoId = 1, Nombre = "PESCAISLA", NombreCompleto = "Empresa Pesquera Industrial Isla de la Juventud PESCAISLA" },
            /* 58 */ new EmpresaApiModel { Id = 7969, DivisionId = 3, GrupoId = 1, Nombre = "FLOGOLFO", NombreCompleto = "Empresa Lanchera Flota del Golfo FLOGOLFO" },
            /* 59 */ new EmpresaApiModel { Id = 12534, DivisionId = 3, GrupoId = 1, Nombre = "EPICOL", NombreCompleto = "Empresa Pesquera Industrial de La Coloma EPICOL" },
            /* 60 */ new EmpresaApiModel { Id = 12535, DivisionId = 3, GrupoId = 1, Nombre = "EPICAI", NombreCompleto = "Empresa Pesquera Industrial de Caibarién EPICAI" },
            /* 61 */ new EmpresaApiModel { Id = 12536, DivisionId = 3, GrupoId = 1, Nombre = "EPISAN", NombreCompleto = "Empresa Pesquera Industrial de Sancti Spíritus EPISAN" },
            /* 62 */ new EmpresaApiModel { Id = 12537, DivisionId = 3, GrupoId = 1, Nombre = "EPISUR", NombreCompleto = "Empresa Pesquera Industrial de Santa Cruz del Sur EPISUR" },
            /* 63 */ new EmpresaApiModel { Id = 12538, DivisionId = 3, GrupoId = 1, Nombre = "EPIVILA", NombreCompleto = "Empresa Pesquera Industrial de Ciego de Ávila EPIVILA" },
            /* 64 */ new EmpresaApiModel { Id = 12539, DivisionId = 3, GrupoId = 1, Nombre = "EPIGRAM", NombreCompleto = "Empresa Pesquera Industrial de Granma EPIGRAM" },
            /* 65 */ new EmpresaApiModel { Id = 12540, DivisionId = 3, GrupoId = 1, Nombre = "EPICIEN", NombreCompleto = "Empresa Pesquera Industrial de Cienfuegos EPICIEN" },
            /* 66 */ //new EmpresaApiModel { Id = 12801, DivisionId = 3, GrupoId = 1, Nombre = "EPINIQ", NombreCompleto = "Empresa Pesquera Industrial de Niquero EPINIQ" },

            /* 67 */ new EmpresaApiModel { Id = 7430, DivisionId = 3, GrupoId = 2, Nombre = "PESCARIO", NombreCompleto = "Empresa Pesquera de Pinar del Río PESCARIO" },
            /* 68 */ new EmpresaApiModel { Id = 7462, DivisionId = 3, GrupoId = 2, Nombre = "PESCAVILA", NombreCompleto = "Empresa Pesquera de Ciego de Ávila PESCAVILA" },
            /* 69 */ new EmpresaApiModel { Id = 7491, DivisionId = 3, GrupoId = 2, Nombre = "PESCAHOL", NombreCompleto = "Empresa Pesquera de Holguín PESCAHOL" },
            /* 70 */ new EmpresaApiModel { Id = 7541, DivisionId = 3, GrupoId = 2, Nombre = "PESCAMAT", NombreCompleto = "Empresa Pesquera de Matanzas PESCAMAT" },
            /* 71 */ new EmpresaApiModel { Id = 7582, DivisionId = 3, GrupoId = 2, Nombre = "PESCASAN", NombreCompleto = "Empresa Pesquera de Santiago de Cuba PESCASAN" },
            /* 72 */ new EmpresaApiModel { Id = 7597, DivisionId = 3, GrupoId = 2, Nombre = "PESCAVILLA", NombreCompleto = "Empresa Pesquera de Villa Clara PESCAVILLA" },
            /* 73 */ new EmpresaApiModel { Id = 7622, DivisionId = 3, GrupoId = 2, Nombre = "PESCASPIR", NombreCompleto = "Empresa Pesquera de Sancti Spíritus PESCASPIR" },
            /* 74 */ new EmpresaApiModel { Id = 7649, DivisionId = 3, GrupoId = 2, Nombre = "PESCACAM", NombreCompleto = "Empresa Pesquera de Camagüey PESCACAM" },
            /* 75 */ new EmpresaApiModel { Id = 7881, DivisionId = 3, GrupoId = 2, Nombre = "ACUABANA", NombreCompleto = "Empresa Pesquera de La Habana ACUABANA" },
            /* 76 */ new EmpresaApiModel { Id = 7888, DivisionId = 3, GrupoId = 2, Nombre = "EDTA", NombreCompleto = "Preparación Acuícola de Mampostón EDTA" },
            /* 77 */ new EmpresaApiModel { Id = 7911, DivisionId = 3, GrupoId = 2, Nombre = "PESCAGRAN", NombreCompleto = "Empresa Pesquera de Granma PESCAGRAN" },
            /* 78 */ new EmpresaApiModel { Id = 14700, DivisionId = 3, GrupoId = 2, Nombre = "PESCATUN", NombreCompleto = "Empresa Pesquera de Las Tunas PESCATUN" },
            /* 79 */ new EmpresaApiModel { Id = 14699, DivisionId = 3, GrupoId = 2, Nombre = "PESCAGUAN", NombreCompleto = "Empresa Pesquera de Guantánamo PESCAGUÁN" },

            /* 80 */ new EmpresaApiModel { Id = 14202, DivisionId = 3, GrupoId = 3, Nombre = "GDECAN", NombreCompleto = "Empresa para el cultivo del Camarón GDECAN" },
            /* 81 */ new EmpresaApiModel { Id = 13051, DivisionId = 3, GrupoId = 3, Nombre = "CARIBEX", NombreCompleto = "Empresa Comercial CARIBEX" },
            /* 82 */ new EmpresaApiModel { Id = 4168, DivisionId = 3, GrupoId = 3, Nombre = "Pesca Caribe", NombreCompleto = "Empresa Pesca Caribe" },
            /* 83 */ new EmpresaApiModel { Id = 7955, DivisionId = 3, GrupoId = 3, Nombre = "PROPES", NombreCompleto = "Empresa Proveedora del Minal PROPES" },
            /* 84 */ new EmpresaApiModel { Id = 11307, DivisionId = 3, GrupoId = 3, Nombre = "COPMAR", NombreCompleto = "Empresa Comercial de Alimentos COPMAR" },
            /* 85 */ new EmpresaApiModel { Id = 11163, DivisionId = 3, GrupoId = 3, Nombre = "PRODAL", NombreCompleto = "Empresa Productora de Alimentos de Regla PRODAL" },
            /* 86 */ new EmpresaApiModel { Id = 7980, DivisionId = 3, GrupoId = 3, Nombre = "TERREF", NombreCompleto = "Empresa Terminal Refrigerada TERREF" },
            /* 87 */ new EmpresaApiModel { Id = 7536, DivisionId = 3, GrupoId = 3, Nombre = "ATLAS", NombreCompleto = "Empresa de Transporte Refrigerado ATLAS" },

            /* 88 */ new EmpresaApiModel { Id = 6223, DivisionId = 4, GrupoId = 1, Nombre = "CEPRONA", NombreCompleto = "Proyectos de Construcciones y Servicios Navales CEPRONA" },
            /* 89 */ new EmpresaApiModel { Id = 12070, DivisionId = 4, GrupoId = 1, Nombre = "IDS", NombreCompleto = "Empresa de Diseño y Servicios de Ingeniería IDS" },
            /* 90 */ new EmpresaApiModel { Id = 11721, DivisionId = 4, GrupoId = 1, Nombre = "ESEP", NombreCompleto = "Empresa de Servicios de Seguridad y Protección ESEP" },
            /* 91 */ new EmpresaApiModel { Id = 6228, DivisionId = 4, GrupoId = 1, Nombre = "ALIMATIC", NombreCompleto = "Empresa de Sistemas Automatizados ALIMATIC" },
            /* 92 */ new EmpresaApiModel { Id = 7744, DivisionId = 4, GrupoId = 1, Nombre = "SERIC", NombreCompleto = "Empresa de Refrigeración y Calderas del Minal SERIC" },
            /* 93 */ new EmpresaApiModel { Id = 7730, DivisionId = 4, GrupoId = 1, Nombre = "COMELEC", NombreCompleto = "Empresa de Construcciones Metálicas y Eléctricas COMELEC" },
            /* 94 */ new EmpresaApiModel { Id = 6301, DivisionId = 4, GrupoId = 1, Nombre = "EMSERVA", NombreCompleto = "Empresa de Servicios Varios del Minal EMSERVA" },
            /* 95 */ new EmpresaApiModel { Id = 2046, DivisionId = 4, GrupoId = 1, Nombre = "Revista Mar y Pesca", NombreCompleto = "Empresa Revista Mar y Pesca" },
            /* 96 */ new EmpresaApiModel { Id = 13541, DivisionId = 4, GrupoId = 1, Nombre = "ALIMPEX", NombreCompleto = "Empresa Importadora Exportadora del Minal ALIMPEX" },
            /* 97 */ new EmpresaApiModel { Id = 14097, DivisionId = 4, GrupoId = 1, Nombre = "GEIA", NombreCompleto = "Grupo Empresarial de la Industria Alimentaria" },
            /* 98 */ new EmpresaApiModel { Id = 14091, DivisionId = 4, GrupoId = 1, Nombre = "OSDE GEIA", NombreCompleto = "OSDE Grupo Empresarial de la Industria Alimentaria" },

            // TODO: Borrar
            /* 98 */ new EmpresaApiModel { Id = 111, DivisionId = 3, GrupoId = 2, Nombre = "Error_111", NombreCompleto = "Error_111" },
        };
    }
}
