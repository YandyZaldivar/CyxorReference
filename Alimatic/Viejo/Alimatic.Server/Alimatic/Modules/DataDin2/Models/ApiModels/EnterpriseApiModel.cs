namespace Alimatic.DataDin2.Models
{
    public class EnterpriseApiModel
    {
        public int Id { get; set; }

        // [AutoMapper.IgnoreMap] //NOTE: This is working!!
        public int GroupId { get; set; }
        public int DivisionId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public static EnterpriseApiModel[] Enterprises { get; } = new EnterpriseApiModel[]
        {
            // Alimentaria

            /* 01 */ new EnterpriseApiModel { Id = 1580, DivisionId = 1, GroupId = 1, Name = "Cárnica Pinar del Río", FullName = "Empresa cárnica Pinar del Río" },
            /* 02 */ new EnterpriseApiModel { Id = 1585, DivisionId = 1, GroupId = 1, Name = "Cárnica Matanzas", FullName = "Empresa cárnica Matanzas" },
            /* 03 */ new EnterpriseApiModel { Id = 1587, DivisionId = 1, GroupId = 1, Name = "Cárnica Villa Clara", FullName = "Empresa cárnica Villa Clara" },
            /* 04 */ new EnterpriseApiModel { Id = 1588, DivisionId = 1, GroupId = 1, Name = "Cárnica Sancti Spíritus", FullName = "Empresa cárnica Sancti Spíritus" },
            /* 05 */ new EnterpriseApiModel { Id = 1589, DivisionId = 1, GroupId = 1, Name = "Cárnica Camagüey", FullName = "Empresa cárnica Camagüey" },
            /* 06 */ new EnterpriseApiModel { Id = 1591, DivisionId = 1, GroupId = 1, Name = "Cárnica Holguín", FullName = "Empresa cárnica Holguín" },
            /* 07 */ new EnterpriseApiModel { Id = 1592, DivisionId = 1, GroupId = 1, Name = "Cárnica Granma", FullName = "Empresa cárnica Granma" },
            /* 08 */ new EnterpriseApiModel { Id = 1593, DivisionId = 1, GroupId = 1, Name = "Cárnica Santiago de Cuba", FullName = "Empresa cárnica Santiago" },
            /* 09 */ new EnterpriseApiModel { Id = 1954, DivisionId = 1, GroupId = 1, Name = "Cárnica Guantánamo", FullName = "Empresa cárnica Guantánamo" },
            /* 10 */ new EnterpriseApiModel { Id = 1961, DivisionId = 1, GroupId = 1, Name = "Cárnica Ciego de Ávila", FullName = "Empresa cárnica Ciego de Ávila" },
            /* 11 */ new EnterpriseApiModel { Id = 2660, DivisionId = 1, GroupId = 1, Name = "Cárnica Las Tunas", FullName = "Empresa cárnica Las Tunas" },
            /* 12 */ new EnterpriseApiModel { Id = 2664, DivisionId = 1, GroupId = 1, Name = "Cárnica La Habana", FullName = "Empresa cárnica Habana" },
            /* 13 */ new EnterpriseApiModel { Id = 2756, DivisionId = 1, GroupId = 1, Name = "Cárnica Cienfuegos", FullName = "Empresa cárnica Cienfuegos" },
            /* 14 */ new EnterpriseApiModel { Id = 12700, DivisionId = 1, GroupId = 1, Name = "Cárnica Tauro", FullName = "Empresa cárnica Tauro" },
            /* 15 */ new EnterpriseApiModel { Id = 13017, DivisionId = 1, GroupId = 1, Name = "Cárnica Nueva Paz", FullName = "Empresa cárnica Nueva Paz" },
            /* 16 */ new EnterpriseApiModel { Id = 14203, DivisionId = 1, GroupId = 1, Name = "ASECAR", FullName = "Empresa de aseguramiento de la industria cárnica" },

            /* 17 */ new EnterpriseApiModel { Id = 1384, DivisionId = 1, GroupId = 2, Name = "Lácteos Coppelia", FullName = "Empresa de productos lácteos Coppelia" },
            /* 18 */ new EnterpriseApiModel { Id = 1594, DivisionId = 1, GroupId = 2, Name = "Lácteos Pinar del Río", FullName = "Empresa de productos lácteos y confitería Pinar del Río" },
            /* 19 */ new EnterpriseApiModel { Id = 1595, DivisionId = 1, GroupId = 2, Name = "Lácteos Artemisa", FullName = "Empresa de productos lácteos Artemisa" },
            /* 20 */ new EnterpriseApiModel { Id = 1597, DivisionId = 1, GroupId = 2, Name = "Lácteos Aseguramiento", FullName = "Empresa de aseguramiento de la industria láctea" },
            /* 21 */ new EnterpriseApiModel { Id = 1598, DivisionId = 1, GroupId = 2, Name = "Lácteos La Habana", FullName = "Empresa complejo lácteo Habana" },
            /* 22 */ new EnterpriseApiModel { Id = 1599, DivisionId = 1, GroupId = 2, Name = "Lácteos Matanzas", FullName = "Empresa de productos lácteos Matanzas" },
            /* 23 */ new EnterpriseApiModel { Id = 1600, DivisionId = 1, GroupId = 2, Name = "Lácteos Villa Clara", FullName = "Empresa de productos lácteos Villa Clara" },
            /* 24 */ new EnterpriseApiModel { Id = 1601, DivisionId = 1, GroupId = 2, Name = "Lácteos Escambray", FullName = "Empresa de productos lácteos Escambray" },
            /* 25 */ new EnterpriseApiModel { Id = 1606, DivisionId = 1, GroupId = 2, Name = "Lácteos Río Zaza", FullName = "Empresa de productos lácteos Río Zaza" },
            /* 26 */ new EnterpriseApiModel { Id = 1607, DivisionId = 1, GroupId = 2, Name = "Lácteos Ciego de Ávila", FullName = "Empresa de productos lácteos Ciego de Ávila" },
            /* 27 */ new EnterpriseApiModel { Id = 1609, DivisionId = 1, GroupId = 2, Name = "Lácteos Camagüey", FullName = "Empresa de productos lácteos Camagüey" },
            /* 28 */ new EnterpriseApiModel { Id = 1610, DivisionId = 1, GroupId = 2, Name = "Lácteos Las Tunas", FullName = "Empresa de productos lácteos Las Tunas" },
            /* 29 */ new EnterpriseApiModel { Id = 1611, DivisionId = 1, GroupId = 2, Name = "Lácteos Holguín", FullName = "Empresa de productos lácteos Holguín" },
            /* 30 */ new EnterpriseApiModel { Id = 1612, DivisionId = 1, GroupId = 2, Name = "Lácteos Bayamo", FullName = "Empresa de productos lácteos Bayamo" },
            /* 31 */ new EnterpriseApiModel { Id = 1615, DivisionId = 1, GroupId = 2, Name = "Lácteos Santiago de Cuba", FullName = "Empresa de productos lácteos Santiago de Cuba" },
            /* 32 */ new EnterpriseApiModel { Id = 1616, DivisionId = 1, GroupId = 2, Name = "Lácteos Guantánamo", FullName = "Empresa de productos lácteos Guantánamo" },

            /* 33 */ new EnterpriseApiModel { Id = 14215, DivisionId = 1, GroupId = 3, Name = "Conservas de Vegetales", FullName = "Empresa de conservas de vegetales" },
            /* 34 */ new EnterpriseApiModel { Id = 14216, DivisionId = 1, GroupId = 3, Name = "Torrefactora de Café", FullName = "Empresa de torrefacción y comercialización de café" },
            /* 35 */ new EnterpriseApiModel { Id = 4272, DivisionId = 1, GroupId = 3, Name = "Alimentos Isla", FullName = "Empresa productora de alimentos Isla de la Juventud" },

            /* 36 */ new EnterpriseApiModel { Id = 1663, DivisionId = 2, GroupId = 1, Name = "EMBER La Habana", FullName = "Empresa de bebidas y refrescos La Habana" },
            /* 37 */ new EnterpriseApiModel { Id = 1665, DivisionId = 2, GroupId = 1, Name = "EMBER Villa Clara", FullName = "Empresa de bebidas y refrescos Villa Clara" },
            /* 38 */ new EnterpriseApiModel { Id = 1666, DivisionId = 2, GroupId = 1, Name = "EMBER Camagüey", FullName = "Empresa de bebidas y refrescos Camagüey" },
            /* 39 */ new EnterpriseApiModel { Id = 1667, DivisionId = 2, GroupId = 1, Name = "EMBER Granma", FullName = "Empresa de bebidas y refrescos Granma" },
            /* 40 */ new EnterpriseApiModel { Id = 1668, DivisionId = 2, GroupId = 1, Name = "EMBER Santiago de Cuba", FullName = "Empresa de bebidas y refrescos Santiago de Cuba" },
            /* 41 */ new EnterpriseApiModel { Id = 1674, DivisionId = 2, GroupId = 1, Name = "EMBER Pinar del Río", FullName = "Empresa de bebidas y refrescos Pinar del Río" },
            /* 42 */ new EnterpriseApiModel { Id = 1679, DivisionId = 2, GroupId = 1, Name = "EMBER Ciego de Ávila", FullName = "Empresa de bebidas y refrescos Ciego de Ávila" },
            /* 43 */ new EnterpriseApiModel { Id = 4796, DivisionId = 2, GroupId = 1, Name = "EMBER Mayabeque", FullName = "Empresa de bebidas y refrescos Mayabeque" },
            /* 44 */ new EnterpriseApiModel { Id = 13042, DivisionId = 2, GroupId = 1, Name = "EMBER Aseguramiento", FullName = "Empresa aseguramiento industrias bebidas y refrescos" },

            /* 45 */ new EnterpriseApiModel { Id = 1672, DivisionId = 2, GroupId = 2, Name = "Cervecería Manacas", FullName = "Empresa cervecería Antonio Díaz Santana (Manacas)" },
            /* 46 */ new EnterpriseApiModel { Id = 3008, DivisionId = 2, GroupId = 2, Name = "Cervecería Modelo", FullName = "Empresa cervecería Guido Pérez (Modelo)" },
            /* 47 */ new EnterpriseApiModel { Id = 4175, DivisionId = 2, GroupId = 2, Name = "Cervecería Hatuey", FullName = "Empresa cervecería Santiago de Cuba (Hatuey)" },
            /* 48 */ new EnterpriseApiModel { Id = 11724, DivisionId = 2, GroupId = 2, Name = "Cervecería Tinima", FullName = "Empresa cervecería Tinima" },

            /* 49 */ new EnterpriseApiModel { Id = 1617, DivisionId = 2, GroupId = 3, Name = "Aceites La Habana", FullName = "Empresa de aceites y grasas comestibles La Habana" },
            /* 50 */ new EnterpriseApiModel { Id = 13480, DivisionId = 2, GroupId = 3, Name = "Aceites Camagüey", FullName = "Empresa de aceites y grasas comestibles Camagüey" },
            /* 51 */ new EnterpriseApiModel { Id = 2794, DivisionId = 2, GroupId = 3, Name = "Erasol", FullName = "Empresa refinadora de aceites de Santiago de Cuba (Erasol)" },
            /* 52 */ new EnterpriseApiModel { Id = 13819, DivisionId = 2, GroupId = 3, Name = "PDS", FullName = "Empresa procesadora de soya (PDS)" },

            /* 53 */ new EnterpriseApiModel { Id = 1734, DivisionId = 2, GroupId = 4, Name = "Molinera", FullName = "Empresa cubana de molinería" },
            /* 54 */ new EnterpriseApiModel { Id = 14194, DivisionId = 2, GroupId = 4, Name = "Confitera", FullName = "Empresa de confitería y derivados de la harina" },
            /* 55 */ new EnterpriseApiModel { Id = 11989, DivisionId = 2, GroupId = 4, Name = "Cubana del Pan", FullName = "Empresa cubana del pan" },

            /* 56 */ new EnterpriseApiModel { Id = 7508, DivisionId = 3, GroupId = 1, Name = "PESCAHABANA", FullName = "Pesquera Industrial de Batabanó PESCAHABANA" },
            /* 57 */ new EnterpriseApiModel { Id = 7892, DivisionId = 3, GroupId = 1, Name = "PESCAISLA", FullName = "Empresa Pesquera Industrial Isla de la Juventud PESCAISLA" },
            /* 58 */ new EnterpriseApiModel { Id = 7969, DivisionId = 3, GroupId = 1, Name = "FLOGOLFO", FullName = "Empresa Lanchera Flota del Golfo FLOGOLFO" },
            /* 59 */ new EnterpriseApiModel { Id = 12534, DivisionId = 3, GroupId = 1, Name = "EPICOL", FullName = "Empresa Pesquera Industrial de La Coloma EPICOL" },
            /* 60 */ new EnterpriseApiModel { Id = 12535, DivisionId = 3, GroupId = 1, Name = "EPICAI", FullName = "Empresa Pesquera Industrial de Caibarién EPICAI" },
            /* 61 */ new EnterpriseApiModel { Id = 12536, DivisionId = 3, GroupId = 1, Name = "EPISAN", FullName = "Empresa Pesquera Industrial de Sancti Spíritus EPISAN" },
            /* 62 */ new EnterpriseApiModel { Id = 12537, DivisionId = 3, GroupId = 1, Name = "EPISUR", FullName = "Empresa Pesquera Industrial de Santa Cruz del Sur EPISUR" },
            /* 63 */ new EnterpriseApiModel { Id = 12538, DivisionId = 3, GroupId = 1, Name = "EPIVILA", FullName = "Empresa Pesquera Industrial de Ciego de Ávila EPIVILA" },
            /* 64 */ new EnterpriseApiModel { Id = 12539, DivisionId = 3, GroupId = 1, Name = "EPIGRAM", FullName = "Empresa Pesquera Industrial de Granma EPIGRAM" },
            /* 65 */ new EnterpriseApiModel { Id = 12540, DivisionId = 3, GroupId = 1, Name = "EPICIEN", FullName = "Empresa Pesquera Industrial de Cienfuegos EPICIEN" },
            /* 66 */ //new EmpresaApiModel { Id = 12801, DivisionId = 3, GrupoId = 1, Nombre = "EPINIQ", NombreCompleto = "Empresa Pesquera Industrial de Niquero EPINIQ" },

            /* 67 */ new EnterpriseApiModel { Id = 7430, DivisionId = 3, GroupId = 2, Name = "PESCARIO", FullName = "Empresa Pesquera de Pinar del Río PESCARIO" },
            /* 68 */ new EnterpriseApiModel { Id = 7462, DivisionId = 3, GroupId = 2, Name = "PESCAVILA", FullName = "Empresa Pesquera de Ciego de Ávila PESCAVILA" },
            /* 69 */ new EnterpriseApiModel { Id = 7491, DivisionId = 3, GroupId = 2, Name = "PESCAHOL", FullName = "Empresa Pesquera de Holguín PESCAHOL" },
            /* 70 */ new EnterpriseApiModel { Id = 7541, DivisionId = 3, GroupId = 2, Name = "PESCAMAT", FullName = "Empresa Pesquera de Matanzas PESCAMAT" },
            /* 71 */ new EnterpriseApiModel { Id = 7582, DivisionId = 3, GroupId = 2, Name = "PESCASAN", FullName = "Empresa Pesquera de Santiago de Cuba PESCASAN" },
            /* 72 */ new EnterpriseApiModel { Id = 7597, DivisionId = 3, GroupId = 2, Name = "PESCAVILLA", FullName = "Empresa Pesquera de Villa Clara PESCAVILLA" },
            /* 73 */ new EnterpriseApiModel { Id = 7622, DivisionId = 3, GroupId = 2, Name = "PESCASPIR", FullName = "Empresa Pesquera de Sancti Spíritus PESCASPIR" },
            /* 74 */ new EnterpriseApiModel { Id = 7649, DivisionId = 3, GroupId = 2, Name = "PESCACAM", FullName = "Empresa Pesquera de Camagüey PESCACAM" },
            /* 75 */ new EnterpriseApiModel { Id = 7881, DivisionId = 3, GroupId = 2, Name = "ACUABANA", FullName = "Empresa Pesquera de La Habana ACUABANA" },
            /* 76 */ new EnterpriseApiModel { Id = 7888, DivisionId = 3, GroupId = 2, Name = "EDTA", FullName = "Preparación Acuícola de Mampostón EDTA" },
            /* 77 */ new EnterpriseApiModel { Id = 7911, DivisionId = 3, GroupId = 2, Name = "PESCAGRAN", FullName = "Empresa Pesquera de Granma PESCAGRAN" },
            /* 78 */ new EnterpriseApiModel { Id = 14700, DivisionId = 3, GroupId = 2, Name = "PESCATUN", FullName = "Empresa Pesquera de Las Tunas PESCATUN" },
            /* 79 */ new EnterpriseApiModel { Id = 14699, DivisionId = 3, GroupId = 2, Name = "PESCAGUAN", FullName = "Empresa Pesquera de Guantánamo PESCAGUÁN" },

            /* 80 */ new EnterpriseApiModel { Id = 14202, DivisionId = 3, GroupId = 3, Name = "GDECAN", FullName = "Empresa para el cultivo del Camarón GDECAN" },
            /* 81 */ new EnterpriseApiModel { Id = 13051, DivisionId = 3, GroupId = 3, Name = "CARIBEX", FullName = "Empresa Comercial CARIBEX" },
            /* 82 */ new EnterpriseApiModel { Id = 4168, DivisionId = 3, GroupId = 3, Name = "Pesca Caribe", FullName = "Empresa Pesca Caribe" },
            /* 83 */ new EnterpriseApiModel { Id = 7955, DivisionId = 3, GroupId = 3, Name = "PROPES", FullName = "Empresa Proveedora del Minal PROPES" },
            /* 84 */ new EnterpriseApiModel { Id = 11307, DivisionId = 3, GroupId = 3, Name = "COPMAR", FullName = "Empresa Comercial de Alimentos COPMAR" },
            /* 85 */ new EnterpriseApiModel { Id = 11163, DivisionId = 3, GroupId = 3, Name = "PRODAL", FullName = "Empresa Productora de Alimentos de Regla PRODAL" },
            /* 86 */ new EnterpriseApiModel { Id = 7980, DivisionId = 3, GroupId = 3, Name = "TERREF", FullName = "Empresa Terminal Refrigerada TERREF" },
            /* 87 */ new EnterpriseApiModel { Id = 7536, DivisionId = 3, GroupId = 3, Name = "ATLAS", FullName = "Empresa de Transporte Refrigerado ATLAS" },

            /* 88 */ new EnterpriseApiModel { Id = 6223, DivisionId = 4, GroupId = 1, Name = "CEPRONA", FullName = "Proyectos de Construcciones y Servicios Navales CEPRONA" },
            /* 89 */ new EnterpriseApiModel { Id = 12070, DivisionId = 4, GroupId = 1, Name = "IDS", FullName = "Empresa de Diseño y Servicios de Ingeniería IDS" },
            /* 90 */ new EnterpriseApiModel { Id = 11721, DivisionId = 4, GroupId = 1, Name = "ESEP", FullName = "Empresa de Servicios de Seguridad y Protección ESEP" },
            /* 91 */ new EnterpriseApiModel { Id = 6228, DivisionId = 4, GroupId = 1, Name = "ALIMATIC", FullName = "Empresa de Sistemas Automatizados ALIMATIC" },
            /* 92 */ new EnterpriseApiModel { Id = 7744, DivisionId = 4, GroupId = 1, Name = "SERIC", FullName = "Empresa de Refrigeración y Calderas del Minal SERIC" },
            /* 93 */ new EnterpriseApiModel { Id = 7730, DivisionId = 4, GroupId = 1, Name = "COMELEC", FullName = "Empresa de Construcciones Metálicas y Eléctricas COMELEC" },
            /* 94 */ new EnterpriseApiModel { Id = 6301, DivisionId = 4, GroupId = 1, Name = "EMSERVA", FullName = "Empresa de Servicios Varios del Minal EMSERVA" },
            /* 95 */ new EnterpriseApiModel { Id = 2046, DivisionId = 4, GroupId = 1, Name = "Revista Mar y Pesca", FullName = "Empresa Revista Mar y Pesca" },
            /* 96 */ new EnterpriseApiModel { Id = 13541, DivisionId = 4, GroupId = 1, Name = "ALIMPEX", FullName = "Empresa Importadora Exportadora del Minal ALIMPEX" },
            /* 97 */ new EnterpriseApiModel { Id = 14097, DivisionId = 4, GroupId = 1, Name = "GEIA", FullName = "Grupo Empresarial de la Industria Alimentaria" },
            /* 98 */ new EnterpriseApiModel { Id = 14091, DivisionId = 4, GroupId = 1, Name = "OSDE GEIA", FullName = "OSDE Grupo Empresarial de la Industria Alimentaria" },
        };
    }
}
