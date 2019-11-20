/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

namespace Halo.Models
{
    using Cyxor.Models;

    public class HospitalApiModel : IdNombreApiModel
    {
        public int ProvinciaId { get; set; }

        public static HospitalApiModel[] Hospitales { get; } = new HospitalApiModel[]
        {
            new HospitalApiModel { Id = 1, ProvinciaId = 1, Nombre = "Abel Santamaría Cuadrado" },
            new HospitalApiModel { Id = 2, ProvinciaId = 1, Nombre = "Augusto César Sandino" },
            new HospitalApiModel { Id = 3, ProvinciaId = 1, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 4, ProvinciaId = 2, Nombre = "Iván Portuondo" },
            new HospitalApiModel { Id = 5, ProvinciaId = 2, Nombre = "Ciro Redondo" },
            new HospitalApiModel { Id = 6, ProvinciaId = 2, Nombre = "Comandante Pinares" },
            new HospitalApiModel { Id = 7, ProvinciaId = 2, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 8, ProvinciaId = 3, Nombre = "Ramón González Coro" },
            new HospitalApiModel { Id = 9, ProvinciaId = 3, Nombre = "América Arias" },
            new HospitalApiModel { Id = 10, ProvinciaId = 3, Nombre = "Luis Díaz Soto" },
            new HospitalApiModel { Id = 11, ProvinciaId = 3, Nombre = "Ángel Arturo Aballí" },
            new HospitalApiModel { Id = 12, ProvinciaId = 3, Nombre = "Materno de Guanabacoa" },
            new HospitalApiModel { Id = 13, ProvinciaId = 3, Nombre = "Materno 10 de Octubre" },
            new HospitalApiModel { Id = 14, ProvinciaId = 3, Nombre = "Eusebio Hernández" },
            new HospitalApiModel { Id = 15, ProvinciaId = 3, Nombre = "Enrique Cabrera" },
            new HospitalApiModel { Id = 16, ProvinciaId = 3, Nombre = "Hermanos Ameijeiras" },
            new HospitalApiModel { Id = 17, ProvinciaId = 3, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 18, ProvinciaId = 4, Nombre = "Piti Fajardo" },
            new HospitalApiModel { Id = 19, ProvinciaId = 4, Nombre = "Leopoldito Martínez" },
            new HospitalApiModel { Id = 20, ProvinciaId = 4, Nombre = "Aleida Fernández" },
            new HospitalApiModel { Id = 21, ProvinciaId = 4, Nombre = "Policlínico Alberto Fernández" },
            new HospitalApiModel { Id = 22, ProvinciaId = 4, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 23, ProvinciaId = 5, Nombre = "Julio R. Alfonso" },
            new HospitalApiModel { Id = 24, ProvinciaId = 5, Nombre = "Faustino Pérez" },
            new HospitalApiModel { Id = 25, ProvinciaId = 5, Nombre = "Julio M. Aróstegui" },
            new HospitalApiModel { Id = 26, ProvinciaId = 5, Nombre = "Mario Muñoz Monroy" },
            new HospitalApiModel { Id = 27, ProvinciaId = 5, Nombre = "Iluminado Rodríguez" },
            new HospitalApiModel { Id = 28, ProvinciaId = 5, Nombre = "Pedro Betancourt" },
            new HospitalApiModel { Id = 29, ProvinciaId = 5, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 30, ProvinciaId = 6, Nombre = "Gustavo Aldereguía Lima" },
            new HospitalApiModel { Id = 31, ProvinciaId = 6, Nombre = "Otras unidades de salud" },
        
            new HospitalApiModel { Id = 32, ProvinciaId = 7, Nombre = "Mariana Grajales" },
            new HospitalApiModel { Id = 33, ProvinciaId = 7, Nombre = "Mártires del 9 de Abril" },
            new HospitalApiModel { Id = 34, ProvinciaId = 7, Nombre = "General de Placetas" },
            new HospitalApiModel { Id = 35, ProvinciaId = 7, Nombre = "General de Remedios" },
            new HospitalApiModel { Id = 36, ProvinciaId = 7, Nombre = "Arnaldo Milián Castro" },
            new HospitalApiModel { Id = 37, ProvinciaId = 7, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 38, ProvinciaId = 8, Nombre = "Joaquín Paneca" },
            new HospitalApiModel { Id = 39, ProvinciaId = 8, Nombre = "Materno Infantil de Cabaiguán" },
            new HospitalApiModel { Id = 40, ProvinciaId = 8, Nombre = "General de Fomento" },
            new HospitalApiModel { Id = 41, ProvinciaId = 8, Nombre = "General de Trinidad" },
            new HospitalApiModel { Id = 42, ProvinciaId = 8, Nombre = "Camilo Cienfuegos" },
            new HospitalApiModel { Id = 43, ProvinciaId = 8, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 44, ProvinciaId = 9, Nombre = "Roberto Rodriguez" },
            new HospitalApiModel { Id = 45, ProvinciaId = 9, Nombre = "Antonio Luaces Iraola" },
            new HospitalApiModel { Id = 46, ProvinciaId = 9, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 47, ProvinciaId = 10, Nombre = "Ana Betancourt" },
            new HospitalApiModel { Id = 48, ProvinciaId = 10, Nombre = "Piti Fajardo" },
            new HospitalApiModel { Id = 49, ProvinciaId = 10, Nombre = "General de Nuevitas" },
            new HospitalApiModel { Id = 50, ProvinciaId = 10, Nombre = "Armando Cardoso" },
            new HospitalApiModel { Id = 51, ProvinciaId = 10, Nombre = "José Espiridón" },
            new HospitalApiModel { Id = 52, ProvinciaId = 10, Nombre = "Amado Fernández" },
            new HospitalApiModel { Id = 53, ProvinciaId = 10, Nombre = "Manuel Ascunce" },
            new HospitalApiModel { Id = 54, ProvinciaId = 10, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 55, ProvinciaId = 11, Nombre = "Ernesto Guevara" },
            new HospitalApiModel { Id = 56, ProvinciaId = 11, Nombre = "Guillermo Domínguez" },
            new HospitalApiModel { Id = 57, ProvinciaId = 11, Nombre = "Luis Adana Palomino" },
            new HospitalApiModel { Id = 58, ProvinciaId = 11, Nombre = "14 de Junio" },
            new HospitalApiModel { Id = 59, ProvinciaId = 11, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 60, ProvinciaId = 12, Nombre = "Gustavo Aldereguía" },
            new HospitalApiModel { Id = 61, ProvinciaId = 12, Nombre = "Luis Mario Cruz Cruz" },
            new HospitalApiModel { Id = 62, ProvinciaId = 12, Nombre = "Vladimir I. Lenin" },
            new HospitalApiModel { Id = 63, ProvinciaId = 12, Nombre = "Nicodemus Regalado" },
            new HospitalApiModel { Id = 64, ProvinciaId = 12, Nombre = "Faustino Borrero" },
            new HospitalApiModel { Id = 65, ProvinciaId = 12, Nombre = "Mártires de Mayarí" },
            new HospitalApiModel { Id = 66, ProvinciaId = 12, Nombre = "Juan Paz Camejo" },
            new HospitalApiModel { Id = 67, ProvinciaId = 12, Nombre = "Guillermo L. Fernández" },
            new HospitalApiModel { Id = 68, ProvinciaId = 12, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 69, ProvinciaId = 13, Nombre = "Mártires de Jiguaní" },
            new HospitalApiModel { Id = 70, ProvinciaId = 13, Nombre = "Carlos Manuel de Céspedes" },
            new HospitalApiModel { Id = 71, ProvinciaId = 13, Nombre = "Celia Sánchez" },
            new HospitalApiModel { Id = 72, ProvinciaId = 13, Nombre = "Fé del Valle" },
            new HospitalApiModel { Id = 73, ProvinciaId = 13, Nombre = "Gelacio Calaña" },
            new HospitalApiModel { Id = 74, ProvinciaId = 13, Nombre = "Felix Lugones" },
            new HospitalApiModel { Id = 75, ProvinciaId = 13, Nombre = "Mariano Pérez Bali" },
            new HospitalApiModel { Id = 76, ProvinciaId = 13, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 77, ProvinciaId = 14, Nombre = "Orlando Pantoja" },
            new HospitalApiModel { Id = 78, ProvinciaId = 14, Nombre = "Ezequiel Miranda" },
            new HospitalApiModel { Id = 79, ProvinciaId = 14, Nombre = "Alberto Fernández Montes de Oca" },
            new HospitalApiModel { Id = 80, ProvinciaId = 14, Nombre = "Emilio Bárcenas" },
            new HospitalApiModel { Id = 81, ProvinciaId = 14, Nombre = "Roberto Infante" },
            new HospitalApiModel { Id = 82, ProvinciaId = 14, Nombre = "Juan Bruno Zayas" },
            new HospitalApiModel { Id = 83, ProvinciaId = 14, Nombre = "Tamara Bunke" },
            new HospitalApiModel { Id = 84, ProvinciaId = 14, Nombre = "Mariana Grajales" },
            new HospitalApiModel { Id = 85, ProvinciaId = 14, Nombre = "Nelia Delfín" },
            new HospitalApiModel { Id = 86, ProvinciaId = 14, Nombre = "Saturnino Lora" },
            new HospitalApiModel { Id = 87, ProvinciaId = 14, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 88, ProvinciaId = 15, Nombre = "Agostinho Neto" },
            new HospitalApiModel { Id = 89, ProvinciaId = 15, Nombre = "Octavio de la Concepción" },
            new HospitalApiModel { Id = 90, ProvinciaId = 15, Nombre = "Otras unidades de salud" },

            new HospitalApiModel { Id = 91, ProvinciaId = 16, Nombre = "Héroes del Baire" },
            new HospitalApiModel { Id = 92, ProvinciaId = 16, Nombre = "Otras unidades de salud" },
        };
    }
}
/* { Halo.Server } */
